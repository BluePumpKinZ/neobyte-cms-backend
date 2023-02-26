using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.RemoteHosting.Managers;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsiteManager {

	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;
	private readonly IWriteOnlyWebsiteRepository _writeOnlyWebsiteRepository;
	private readonly RemoteHostingManager _hostingConnectionManager;

	public WebsiteManager (IReadOnlyWebsiteRepository readOnlyWebsiteRepository, IWriteOnlyWebsiteRepository writeOnlyWebsiteRepository, RemoteHostingManager hostingConnectionManager) {
		_readOnlyWebsiteRepository = readOnlyWebsiteRepository;
		_writeOnlyWebsiteRepository = writeOnlyWebsiteRepository;
		_hostingConnectionManager = hostingConnectionManager;
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

}