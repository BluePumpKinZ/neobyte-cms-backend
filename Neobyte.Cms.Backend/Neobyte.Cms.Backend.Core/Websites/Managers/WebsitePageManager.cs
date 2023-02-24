using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Exceptions.RemoteHosting;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Core.Websites.Transformers;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Utils;
using System.Text;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsitePageManager {

	private readonly HtmlTransformer _transformer;
	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;
	private readonly IRemoteHostingProvider _remoteHostingProvider;
	private readonly IWriteOnlyPageRepository _writeOnlyPageRepository;
	private readonly IReadOnlyPageRepository _readOnlyPageRepository;
	private readonly PathUtils _pathUtils;

	public WebsitePageManager (HtmlTransformer transformer, IReadOnlyWebsiteRepository readOnlyWebsiteRepository, IRemoteHostingProvider remoteHostingProvider, IWriteOnlyPageRepository writeOnlyPageRepository, IReadOnlyPageRepository readOnlyPageRepository, PathUtils pathUtils) {
		_transformer = transformer;
		_readOnlyWebsiteRepository = readOnlyWebsiteRepository;
		_remoteHostingProvider = remoteHostingProvider;
		_writeOnlyPageRepository = writeOnlyPageRepository;
		_readOnlyPageRepository = readOnlyPageRepository;
		_pathUtils = pathUtils;
	}

	public async Task<WebsiteCreatePageResponseModel> CreateExistingPageAsync (WebsiteCreatePageRequestModel request) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(request.Id);
		if (website is null)
			return new WebsiteCreatePageResponseModel(false, new string[] { "Website not found" });
		var connection = website.Connection;
		if (connection is null)
			return new WebsiteCreatePageResponseModel(false, new string[] { "Website has no connection" });

		var connector = _remoteHostingProvider.GetConnector(connection);
		var filePath = _pathUtils.Combine(website.HomeFolder, request.Path);
		if (!await connector.FileExistsAsync(filePath))
			return new WebsiteCreatePageResponseModel(false, new string[] { $"File {request.Path} does not exist." });

		var page = new Page(request.Name, request.Path) { Website = website };
		await _writeOnlyPageRepository.CreatePageAsync(page);

		return new WebsiteCreatePageResponseModel(true);
	}

	public async Task<IEnumerable<Page>> GetPagesByWebsiteId (WebsiteId websiteId) {
		return await _readOnlyPageRepository.ReadPagesByWebsiteIdAsync(websiteId);
	}

	public async Task DeletePageAsync (WebsiteId websiteId, PageId pageId) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId);
		var page = await _readOnlyPageRepository.ReadPageByIdAsync(pageId);

		if (website is null)
			throw new WebsiteNotFoundException($"Website {websiteId} not found");
		if (page is null)
			throw new PageNotFoundException($"Page {pageId} not found");

		await _writeOnlyPageRepository.DeletePageAsync(page);
	}

	public async Task<string> RenderPageAsync (WebsiteId websiteId, PageId pageId) {
		var htmlContent = await GetPageSourceAsync(websiteId, pageId);
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId);
		return _transformer.ConstructRenderedWebpage(website!.Domain, htmlContent);
	}

	public async Task<string> GetPageSourceAsync (WebsiteId websiteId, PageId pageId) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId);
		var page = await _readOnlyPageRepository.ReadPageByIdAsync(pageId);

		if (website is null)
			throw new WebsiteNotFoundException($"Website {websiteId} not found");
		if (page is null)
			throw new PageNotFoundException($"Page {pageId} not found");

		var connection = website.Connection;
		if (connection is null)
			throw new WebsiteConnectionNotFoundException($"Website {websiteId} has no connection");

		var connector = _remoteHostingProvider.GetConnector(connection);
		var filePath = _pathUtils.Combine(website.HomeFolder, page.Path);
		if (!await connector.FileExistsAsync(filePath))
			throw new PageFileNotFoundException($"File {filePath} does not exist.");

		return Encoding.UTF8.GetString (await connector.GetFileContentAsync(filePath));
	}

	public async Task PublishPageSource (WebsitePagePublishRequestModel request) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(request.WebsiteId);
		var page = await _readOnlyPageRepository.ReadPageByIdAsync(request.PageId);

		if (website is null)
			throw new WebsiteNotFoundException($"Website {request.WebsiteId} not found");
		if (page is null)
			throw new PageNotFoundException($"Page {request.PageId} not found");

		var connection = website.Connection;
		if (connection is null)
			throw new WebsiteConnectionNotFoundException($"Website {request.WebsiteId} has no connection");

		var connector = _remoteHostingProvider.GetConnector(connection);
		var filePath = _pathUtils.Combine(website.HomeFolder, page.Path);
		await connector.CreateFileAsync(filePath, Encoding.UTF8.GetBytes(request.Source));
	}

	public async Task PublishPageRender (WebsitePagePublishRequestModel request) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(request.WebsiteId);
		var page = await _readOnlyPageRepository.ReadPageByIdAsync(request.PageId);

		if (website is null)
			throw new WebsiteNotFoundException($"Website {request.WebsiteId} not found");
		if (page is null)
			throw new PageNotFoundException($"Page {request.PageId} not found");

		var connection = website.Connection;
		if (connection is null)
			throw new WebsiteConnectionNotFoundException($"Website {request.WebsiteId} has no connection");

		var htmlContent = _transformer.DeconstructRenderedWebPage(request.Source);
		var connector = _remoteHostingProvider.GetConnector(connection);
		var filePath = _pathUtils.Combine(website.HomeFolder, page.Path);
		await connector.CreateFileAsync(filePath, Encoding.UTF8.GetBytes(htmlContent));
	}

}