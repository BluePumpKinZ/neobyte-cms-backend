using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IReadOnlyPageRepository {

	public Task<Page?> ReadPageByIdAsync (PageId pageId);

	public Task<Page?> ReadPageWithWebsiteByIdAsync (PageId pageId);

	public Task<IEnumerable<Page>> ReadPagesByWebsiteIdAsync (WebsiteId websiteId);

}