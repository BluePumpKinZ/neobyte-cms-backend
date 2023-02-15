using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Websites;
using System.IO;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsitePageManager {

	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;
	private readonly IRemoteHostingProvider _remoteHostingProvider;
	private readonly IWriteOnlyPageRepository _writeOnlyPageRepository;
	private readonly IReadOnlyPageRepository _readOnlyPageRepository;

	public WebsitePageManager (IReadOnlyWebsiteRepository readOnlyWebsiteRepository, IRemoteHostingProvider remoteHostingProvider, IWriteOnlyPageRepository writeOnlyPageRepository, IReadOnlyPageRepository readOnlyPageRepository) {
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

	public async Task DeletePageAsync (WebsiteId websiteId, PageId pageId) {
		var website = await _readOnlyWebsiteRepository.GetWebsiteByIdAsync(websiteId);
		var page = await _readOnlyPageRepository.GetPageByIdAsync(pageId);

		if (website is null)
			throw new WebsiteNotFoundException($"Website {websiteId} not found");
		if (page is null)
			throw new PageNotFoundException($"Page {pageId} not found");

		await _writeOnlyPageRepository.DeletePageAsync(page);
	}

}