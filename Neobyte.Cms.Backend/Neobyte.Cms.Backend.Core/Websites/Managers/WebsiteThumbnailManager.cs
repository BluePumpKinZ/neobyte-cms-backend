using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Utils;
using PuppeteerSharp;
using System.Linq;
using Page = Neobyte.Cms.Backend.Domain.Websites.Page;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsiteThumbnailManager {

	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;
	private readonly IReadOnlyPageRepository _readOnlyPageRepository;
	private readonly IRemoteHostingProvider _remoteHostingProvider;
	private readonly PathUtils _pathUtils;
	private readonly BrowserManager _browserManager;
	private readonly IWriteOnlyWebsiteRepository _writeOnlyWebsiteRepository;

	public WebsiteThumbnailManager (IReadOnlyWebsiteRepository readOnlyWebsiteRepository, IReadOnlyPageRepository readOnlyPageRepository, IRemoteHostingProvider remoteHostingProvider, PathUtils pathUtils, BrowserManager browserManager, IWriteOnlyWebsiteRepository writeOnlyWebsiteRepository) {
		_readOnlyWebsiteRepository = readOnlyWebsiteRepository;
		_readOnlyPageRepository = readOnlyPageRepository;
		_remoteHostingProvider = remoteHostingProvider;
		_pathUtils = pathUtils;
		_browserManager = browserManager;
		_writeOnlyWebsiteRepository = writeOnlyWebsiteRepository;
	}

	private async Task<string> GeneratePageThumbnailAsync (Website website, Page page) {
		/*var connector = _remoteHostingProvider.GetConnector(website.Connection!);
		var filePath = _pathUtils.Combine(website.HomeFolder, page.Path);
		var sourceBytes = await connector.GetFileContentAsync(filePath);
		var source = Encoding.UTF8.GetString(sourceBytes);*/

		var bytes = await _browserManager.ExecuteOnBrowserAsync(async b => {

			await using var browserPage = await b.NewPageAsync();
			var navigationtask = browserPage.WaitForNavigationAsync(new NavigationOptions {
				WaitUntil = new WaitUntilNavigation[] {
					WaitUntilNavigation.Networkidle2
				},
			});
			await browserPage.GoToAsync($"http://localhost:5110/api/v1/websites/{website.Id}/pages/{page.Id}/display");
			await navigationtask;
			// await Task.Delay(5000);
			var screenshot = await browserPage.ScreenshotDataAsync();

			return screenshot;
		});

		var thumbnailPath = _pathUtils.Combine(website.UploadFolder, "thumbnail.png");
		var connector = _remoteHostingProvider.GetConnector(website.Connection!);
		await connector.CreateFileAsync(thumbnailPath, bytes);

		return thumbnailPath;
	}

	private async Task<string?> GetWebsiteThumbnailUrlAsync (WebsiteId websiteId) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId)
			?? throw new WebsiteNotFoundException($"Website {websiteId} not found");
		if (website.Thumbnail != null)
			return website.Thumbnail;

		var pages = await _readOnlyPageRepository.ReadPagesByWebsiteIdAsync(websiteId);
		var page = pages.FirstOrDefault();
		if (page == null)
			return null;
		string thumbnailPath = await GeneratePageThumbnailAsync(website, page);
		website.Thumbnail = thumbnailPath;
		await _writeOnlyWebsiteRepository.UpdateWebsiteAsync(website);
		return thumbnailPath;
	}

	public async Task<byte[]> GetWebsiteThumbnailAsync (WebsiteId websiteId) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync (websiteId)
			?? throw new WebsiteNotFoundException ($"Website {websiteId} not found");

		var thumbnail = await GetWebsiteThumbnailUrlAsync(websiteId)
			?? throw new PageNotFoundException($"No pages found for website {websiteId}");
		var connector = _remoteHostingProvider.GetConnector(website.Connection!);
		return await connector.GetFileContentAsync(thumbnail);
	}

}