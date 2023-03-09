using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Utils;
using System.Linq;
using System.Text;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsiteThumbnailManager {

	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;
	private readonly IReadOnlyPageRepository _readOnlyPageRepository;
	private readonly IRemoteHostingProvider _remoteHostingProvider;
	private readonly PathUtils _pathUtils;
	private readonly BrowserManager _browserManager;

	public WebsiteThumbnailManager (IReadOnlyWebsiteRepository readOnlyWebsiteRepository, IReadOnlyPageRepository readOnlyPageRepository, IRemoteHostingProvider remoteHostingProvider, PathUtils pathUtils, BrowserManager browserManager) {
		_readOnlyWebsiteRepository = readOnlyWebsiteRepository;
		_readOnlyPageRepository = readOnlyPageRepository;
		_remoteHostingProvider = remoteHostingProvider;
		_pathUtils = pathUtils;
		_browserManager = browserManager;
	}

	private async Task<string> GeneratePageThumbnailAsync (Website website, Page page) {
		var connector = _remoteHostingProvider.GetConnector(website.Connection!);
		var filePath = _pathUtils.Combine(website.HomeFolder, page.Path);
		var sourceBytes = await connector.GetFileContentAsync(filePath);
		var source = Encoding.UTF8.GetString(sourceBytes);

		var bytes = await _browserManager.ExecuteOnBrowserAsync(async (b) => {

			await using var browserPage = await b.NewPageAsync();
			await browserPage.SetContentAsync(source);
			var screenshot = await browserPage.ScreenshotDataAsync();

			return screenshot;
		});

		var thumbnailPath = _pathUtils.Combine(website.UploadFolder, "thumbnail.png");
		await connector.CreateFileAsync(thumbnailPath, bytes);

		return thumbnailPath;
	}

	private async Task<string?> GetWebsiteThumbnailUrlAsync (Website website) {
		if (website.Thumbnail != null)
			return website.Thumbnail;

		var pages = await _readOnlyPageRepository.ReadPagesByWebsiteIdAsync(website.Id);
		var page = pages.FirstOrDefault();
		if (page == null)
			return null;
		return await GeneratePageThumbnailAsync(website, page);
	}

	public async Task<byte[]> GetWebsiteThumbnailAsync (WebsiteId websiteId) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync (websiteId)
			?? throw new WebsiteNotFoundException ($"Website {websiteId} not found");

		var thumbnail = await GetWebsiteThumbnailUrlAsync(website)
			?? throw new PageNotFoundException($"No pages found for website {websiteId}");
		var connector = _remoteHostingProvider.GetConnector(website.Connection!);
		return await connector.GetFileContentAsync(thumbnail);
	}

}