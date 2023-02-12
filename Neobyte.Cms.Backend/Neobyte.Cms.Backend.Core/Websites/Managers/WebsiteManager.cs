using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsiteManager {

	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;

	public WebsiteManager (IReadOnlyWebsiteRepository readOnlyWebsiteRepository) {
		_readOnlyWebsiteRepository = readOnlyWebsiteRepository;
	}

	public async Task<Website> GetWebsiteById (WebsiteId websiteId) {
		return await _readOnlyWebsiteRepository.GetWebsiteByIdAsync(websiteId);
	}

	public async Task<IEnumerable<Website>> GetAllWebsitesAsync () {
		return await _readOnlyWebsiteRepository.GetAllWebsitesAsync();
	}

}