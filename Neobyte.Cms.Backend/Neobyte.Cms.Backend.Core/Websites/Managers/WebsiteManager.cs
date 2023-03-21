using MoreCSharp.Extensions.System.Collections.Generic;
using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.RemoteHosting.Managers;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsiteManager {

	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;
	private readonly IWriteOnlyWebsiteRepository _writeOnlyWebsiteRepository;
	private readonly IReadOnlyPageRepository _readOnlyPageRepository;
	private readonly IReadOnlySnippetRepository _snippetReadOnlyRepository;
	private readonly IWriteOnlyPageRepository _writeOnlyPageRepository;
	private readonly IWriteOnlySnippetRepository _writeOnlySnippetRepository;
	private readonly IReadOnlyWebsiteAccountRepository _readOnlyWebsiteAccountRepository;
	private readonly IWriteOnlyWebsiteAccountRepository _writeOnlyWebsiteAccountRepository;
	private readonly RemoteHostingManager _hostingConnectionManager;

	public WebsiteManager (IReadOnlyWebsiteRepository readOnlyWebsiteRepository, IWriteOnlyWebsiteRepository writeOnlyWebsiteRepository, RemoteHostingManager hostingConnectionManager, IReadOnlyPageRepository pageRepository, IReadOnlySnippetRepository snippetRepository, IWriteOnlyPageRepository writeOnlyPageRepository, IWriteOnlySnippetRepository writeOnlySnippetRepository, IReadOnlyWebsiteAccountRepository readOnlyWebsiteAccountRepository, IWriteOnlyWebsiteAccountRepository writeOnlyWebsiteAccountRepository) {
		_readOnlyWebsiteRepository = readOnlyWebsiteRepository;
		_writeOnlyWebsiteRepository = writeOnlyWebsiteRepository;
		_hostingConnectionManager = hostingConnectionManager;
		_readOnlyPageRepository = pageRepository;
		_snippetReadOnlyRepository = snippetRepository;
		_writeOnlyPageRepository = writeOnlyPageRepository;
		_writeOnlySnippetRepository = writeOnlySnippetRepository;
		_readOnlyWebsiteAccountRepository = readOnlyWebsiteAccountRepository;
		_writeOnlyWebsiteAccountRepository = writeOnlyWebsiteAccountRepository;
	}

	public async Task<Website> AddWebsiteAsync (WebsiteCreateRequestModel request) {
		var website = new Website(request.Name, request.Domain, request.HomeFolder, request.UploadFolder) {
			Connection = _hostingConnectionManager.FromRequestModel(request)
		};

		return await _writeOnlyWebsiteRepository.CreateWebsiteAsync(website);
	}

	public async Task<Website> GetWebsiteByIdAsync (WebsiteId websiteId) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId)
			?? throw new WebsiteNotFoundException($"Website {websiteId} not found");
		return website;
	}

	public async Task<IEnumerable<Website>> GetAllWebsitesAsync () {
		return await _readOnlyWebsiteRepository.ReadAllWebsitesAsync();
	}

	public async Task<Website> EditWebsiteAsync (WebsiteEditRequestModel request) {

		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(new WebsiteId(request.Id))
			?? throw new WebsiteNotFoundException($"Website {request.Id} not found");

		website.Name = request.Name;
		website.Domain = request.Domain;
		website.HomeFolder = request.HomeFolder;
		website.UploadFolder = request.UploadFolder;

		website.Connection = _hostingConnectionManager.FromRequestModel(request, website.Connection);

		return await _writeOnlyWebsiteRepository.UpdateWebsiteAsync(website);
	}

	public async Task DeleteWebsiteAsync (WebsiteId websiteId) {
		
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId)
			?? throw new WebsiteNotFoundException($"Website {websiteId} not found");

		var pages = await _readOnlyPageRepository.ReadPagesByWebsiteIdAsync(websiteId);
		var snippets = await _snippetReadOnlyRepository.ReadAllSnippetsByWebsiteIdAsync(websiteId);
		var websiteAccounts = await _readOnlyWebsiteAccountRepository.ReadAllWebsiteAccountsAsync(websiteId);

		await pages.ForEachAsync(_writeOnlyPageRepository.DeletePageAsync);
		await snippets.ForEachAsync(_writeOnlySnippetRepository.DeleteSnippetAsync);
		await websiteAccounts.ForEachAsync(_writeOnlyWebsiteAccountRepository.DeleteWebsiteAccountAsync);

		await _writeOnlyWebsiteRepository.DeleteWebsiteAsync(website);
	}

}