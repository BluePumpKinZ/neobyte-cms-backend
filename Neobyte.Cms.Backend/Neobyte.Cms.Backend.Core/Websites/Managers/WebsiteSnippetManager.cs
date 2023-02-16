using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsiteSnippetManager {

	private readonly IReadOnlySnippetRepository _readOnlySnippetRepository;

	public WebsiteSnippetManager (IReadOnlySnippetRepository readOnlySnippetRepository) {
		_readOnlySnippetRepository = readOnlySnippetRepository;
	}

	public async Task<IEnumerable<Snippet>> GetWebsiteSnippetsAsync (WebsiteId websiteId) {
		return await _readOnlySnippetRepository.ReadAllSnippetsByWebsiteId(websiteId);
	}

}