using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Exceptions.RemoteHosting;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Core.Websites.Transformers;
using Neobyte.Cms.Backend.Domain.Websites;
using System.IO;
using System.Text;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsitePageManager {

	private readonly HtmlTransformer _transformer;
	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;
	private readonly IRemoteHostingProvider _remoteHostingProvider;
	private readonly IWriteOnlyPageRepository _writeOnlyPageRepository;
	private readonly IReadOnlyPageRepository _readOnlyPageRepository;

	public WebsitePageManager (HtmlTransformer transformer, IReadOnlyWebsiteRepository readOnlyWebsiteRepository, IRemoteHostingProvider remoteHostingProvider, IWriteOnlyPageRepository writeOnlyPageRepository, IReadOnlyPageRepository readOnlyPageRepository) {
		_transformer = transformer;
		_readOnlyWebsiteRepository = readOnlyWebsiteRepository;
		_remoteHostingProvider = remoteHostingProvider;
		_writeOnlyPageRepository = writeOnlyPageRepository;
		_readOnlyPageRepository = readOnlyPageRepository;
	}

	public async Task<WebsiteCreatePageResponseModel> CreateExistingPageAsync (WebsiteCreatePageRequestModel request) {
		var website = await _readOnlyWebsiteRepository.GetWebsiteByIdAsync(request.Id);
		if (website is null)
			return new WebsiteCreatePageResponseModel(false, new string[] { "Website not found" });
		var connection = website.Connection;
		if (connection is null)
			return new WebsiteCreatePageResponseModel(false, new string[] { "Website has no connection" });

		var connector = _remoteHostingProvider.CreateConnector(connection);
		var filepath = Path.Combine(website.HomeFolder, request.Path);
		if (!connector.FileExists(filepath))
			return new WebsiteCreatePageResponseModel(false, new string[] { $"File {request.Path} does not exist." });

		var page = new Page(request.Name, request.Path) { Website = website };
		await _writeOnlyPageRepository.AddPageAsync(page);

		return new WebsiteCreatePageResponseModel(true);
	}

	public async Task<IEnumerable<Page>> GetPagesByWebsiteId (WebsiteId websiteId) {
		return await _readOnlyPageRepository.GetPagesByWebsiteIdAsync(websiteId);
	}

	public async Task DeletePageAsync (WebsiteId websiteId, PageId pageId) {
		var website = await _readOnlyWebsiteRepository.GetWebsiteByIdAsync(websiteId);
		var page = await _readOnlyPageRepository.GetPageByIdAsync(pageId);

		if (website is null)
			throw new WebsiteNotFoundException($"Website {websiteId} not found");
		if (page is null)
			throw new PageNotFoundException($"Page {pageId} not found");

		await _writeOnlyPageRepository.DeletePageAsync(page);
	}

	public async Task<string> RenderPageAsync (WebsiteId websiteId, PageId pageId) {
		var htmlContent = await GetPageSourceAsync(websiteId, pageId);
		var website = await _readOnlyWebsiteRepository.GetWebsiteByIdAsync(websiteId);
		return _transformer.TransformRenderedWebpage(website!.Domain, htmlContent);
	}

	public async Task<string> GetPageSourceAsync (WebsiteId websiteId, PageId pageId) {
		var website = await _readOnlyWebsiteRepository.GetWebsiteByIdAsync(websiteId);
		var page = await _readOnlyPageRepository.GetPageByIdAsync(pageId);

		if (website is null)
			throw new WebsiteNotFoundException($"Website {websiteId} not found");
		if (page is null)
			throw new PageNotFoundException($"Page {pageId} not found");

		var connection = website.Connection;
		if (connection is null)
			throw new WebsiteConnectionNotFoundException($"Website {websiteId} has no connection");

		var connector = _remoteHostingProvider.CreateConnector(connection);
		var filepath = Path.Combine(website.HomeFolder, page.Path);
		if (!connector.FileExists(filepath))
			throw new PageFileNotFoundException($"File {filepath} does not exist.");

		return Encoding.UTF8.GetString (connector.GetFileContent(filepath));
	}

	public async Task PublishPageSource (PagePublishSourceCreateRequest request) {
		var website = await _readOnlyWebsiteRepository.GetWebsiteByIdAsync(request.WebsiteId);
		var page = await _readOnlyPageRepository.GetPageByIdAsync(request.PageId);

		if (website is null)
			throw new WebsiteNotFoundException($"Website {request.WebsiteId} not found");
		if (page is null)
			throw new PageNotFoundException($"Page {request.PageId} not found");

		var connection = website.Connection;
		if (connection is null)
			throw new WebsiteConnectionNotFoundException($"Website {request.WebsiteId} has no connection");

		var connector = _remoteHostingProvider.CreateConnector(connection);
		var filepath = Path.Combine(website.HomeFolder, page.Path);
		connector.CreateFile(filepath, Encoding.UTF8.GetBytes(request.Source));
	}

}