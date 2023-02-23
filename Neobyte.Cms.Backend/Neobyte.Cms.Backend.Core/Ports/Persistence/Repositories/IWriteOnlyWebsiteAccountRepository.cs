using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IWriteOnlyWebsiteAccountRepository {
	public Task<WebsiteAccount> CreateWebsiteAccountAsync (WebsiteAccount websiteAccount);
	public Task DeleteWebsiteAccountAsync (WebsiteAccount websiteAccount);
}