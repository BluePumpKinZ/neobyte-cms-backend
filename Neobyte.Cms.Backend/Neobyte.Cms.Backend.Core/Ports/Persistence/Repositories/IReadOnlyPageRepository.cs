using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IReadOnlyPageRepository {

	public Task<Page?> GetPageByIdAsync (PageId pageId);

	public Task<IEnumerable<Page>> GetPagesByWebsiteIdAsync (WebsiteId websiteId);

}