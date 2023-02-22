using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories;

public class WriteOnlyWebsiteAccountRepository : IWriteOnlyWebsiteAccountRepository {
	public Task<WebsiteAccount> CreateWebsiteAccountAsync (WebsiteAccount websiteAccount) {
		throw new NotImplementedException();
	}

	public Task DeleteWebsiteAccountAsync (WebsiteAccount websiteAccount) {
		throw new NotImplementedException();
	}
}