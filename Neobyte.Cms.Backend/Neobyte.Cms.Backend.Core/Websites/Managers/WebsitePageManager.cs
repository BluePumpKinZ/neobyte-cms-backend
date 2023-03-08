using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Exceptions.RemoteHosting;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Core.Websites.Transformers;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Utils;
using System;
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
			return new WebsiteCreatePageResponseModel(false, new[] { "Website not found" });
		var connection = website.Connection;
		if (connection is null)
			return new WebsiteCreatePageResponseModel(false, new[] { "Website has no connection" });

		var connector = _remoteHostingProvider.GetConnector(connection);
		var filePath = _pathUtils.Combine(website.HomeFolder, request.Path);
		if (!await connector.FileExistsAsync(filePath))
			return new WebsiteCreatePageResponseModel(false, new[] { $"File {request.Path} does not exist." });

		var page = new Page(request.Name, request.Path) { Website = website };
		await _writeOnlyPageRepository.CreatePageAsync(page);

		return new WebsiteCreatePageResponseModel(true);
	}

	public async Task<WebsiteCreatePageResponseModel> CreateEmptyPageAsync (WebsiteCreatePageRequestModel request) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(request.Id);
		if (website is null)
			return new WebsiteCreatePageResponseModel(false, new[] { "Website not found" });
		var connection = website.Connection;
		if (connection is null)
			return new WebsiteCreatePageResponseModel(false, new[] { "Website has no connection" });

		var connector = _remoteHostingProvider.GetConnector(connection);
		var filePath = _pathUtils.Combine(website.HomeFolder, request.Path);
		if (await connector.FileExistsAsync(filePath) || await connector.FolderExistsAsync(filePath))
			return new WebsiteCreatePageResponseModel(false, new[] { $"Path {request.Path} already in use." });

		var page = new Page (request.Name, request.Path) { Website = website };
		await _writeOnlyPageRepository.CreatePageAsync(page);

		await connector.CreateFileAsync(filePath, Array.Empty<byte>());

		return new WebsiteCreatePageResponseModel (true);
	}

	public async Task<IEnumerable<Page>> GetPagesByWebsiteId (WebsiteId websiteId) {
		return await _readOnlyPageRepository.ReadPagesByWebsiteIdAsync(websiteId);
	}

	public async Task<Page> GetPageByIdAsync (WebsiteId websiteId, PageId pageId) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId);
		var page = await _readOnlyPageRepository.ReadPageWithWebsiteByIdAsync(pageId);

		if (website is null)
			throw new WebsiteNotFoundException($"Website {websiteId} not found");
		if (page is null)
			throw new PageNotFoundException($"Page {pageId} not found");

		if (page.Website!.Id != website.Id)
			throw new PageNotFoundException($"Page {pageId} not found");

		return page;
	}

	public async Task<Page> EditPageAsync (WebsiteEditPageRequestModel request) {
		var page = await GetPageByIdAsync(request.WebsiteId, request.PageId);

		page.Name = request.Name;
		page.Path = request.Path;
		page.Modified = DateTime.UtcNow;

		return await _writeOnlyPageRepository.UpdatePageAsync(page);
	}

	public async Task DeletePageAsync (WebsiteId websiteId, PageId pageId) {
		var page = await GetPageByIdAsync(websiteId, pageId);

		await _writeOnlyPageRepository.DeletePageAsync(page);
	}

	public async Task<string> RenderPageAsync (WebsiteId websiteId, PageId pageId) {
		var htmlContent = await GetPageSourceAsync(websiteId, pageId);
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId);
		return _transformer.ConstructRenderedWebpage(website!.Domain, htmlContent);
	}

	public async Task<string> GetPageSourceAsync (WebsiteId websiteId, PageId pageId) {
		var page = await GetPageByIdAsync(websiteId, pageId);
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId);

		var connection = website!.Connection
			?? throw new WebsiteConnectionNotFoundException($"Website {websiteId} has no connection");

		var connector = _remoteHostingProvider.GetConnector(connection);
		var filePath = _pathUtils.Combine(website.HomeFolder, page.Path);
		if (!await connector.FileExistsAsync(filePath))
			throw new PageFileNotFoundException($"File {filePath} does not exist.");

		return Encoding.UTF8.GetString(await connector.GetFileContentAsync(filePath));
	}

	public async Task PublishPageSource (WebsitePagePublishRequestModel request) {
		var page = await GetPageByIdAsync(request.WebsiteId, request.PageId);
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(request.WebsiteId);

		var connection = website!.Connection
			?? throw new WebsiteConnectionNotFoundException($"Website {request.WebsiteId} has no connection");

		var connector = _remoteHostingProvider.GetConnector(connection);
		var filePath = _pathUtils.Combine(website.HomeFolder, page.Path);
		await connector.CreateFileAsync(filePath, Encoding.UTF8.GetBytes(request.Source));
	}

	public async Task PublishPageRender (WebsitePagePublishRequestModel request) {
		var page = await GetPageByIdAsync(request.WebsiteId, request.PageId);
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(request.WebsiteId);

		var connection = website!.Connection
			?? throw new WebsiteConnectionNotFoundException($"Website {request.WebsiteId} has no connection");

		var htmlContent = _transformer.DeconstructRenderedWebPage(request.Source);
		var connector = _remoteHostingProvider.GetConnector(connection);
		var filePath = _pathUtils.Combine(website.HomeFolder, page.Path);
		await connector.CreateFileAsync(filePath, Encoding.UTF8.GetBytes(htmlContent));
	}

}