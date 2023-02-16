using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IReadOnlySnippetRepository {

	public Task<IEnumerable<Snippet>> ReadAllSnippetsByWebsiteId (WebsiteId websiteId);

}