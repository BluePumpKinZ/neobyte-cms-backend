using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IWriteOnlyWebsiteRepository {

	public Task<Website> CreateWebsiteAsync (Website website);

	public Task<Website> UpdateWebsiteAsync (Website website);

	public Task DeleteWebsiteAsync (Website website);

}