using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IReadOnlySnippetRepository {

	public Task<IEnumerable<Snippet>> ReadAllSnippetsByWebsiteIdAsync (WebsiteId websiteId);

	public Task<Snippet?> ReadSnippetWithWebsiteByIdAsync (SnippetId snippetId);

}