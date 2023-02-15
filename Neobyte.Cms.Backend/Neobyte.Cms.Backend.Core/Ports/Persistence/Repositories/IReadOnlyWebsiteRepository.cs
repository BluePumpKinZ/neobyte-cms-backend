using Neobyte.Cms.Backend.Domain.Websites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IReadOnlyWebsiteRepository {

	public Task<Website?> GetWebsiteByIdAsync (WebsiteId websiteId);

	public Task<IEnumerable<Website>> GetAllWebsitesAsync ();

	public Task<IEnumerable<Page>> GetPagesByWebsiteIdAsync (WebsiteId websiteId);

}