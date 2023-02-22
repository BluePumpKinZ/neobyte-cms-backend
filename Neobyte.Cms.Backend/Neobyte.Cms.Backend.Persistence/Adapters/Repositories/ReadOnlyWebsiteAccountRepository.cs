using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Persistence.EF;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

public class ReadOnlyWebsiteAccountRepository : IReadOnlyWebsiteAccountRepository {
	
	private readonly EFDbContext _ctx;
	
	public ReadOnlyWebsiteAccountRepository (EFDbContext ctx) {
		_ctx = ctx;
	}
	
	public async Task<IEnumerable<Account>> ReadAccountsByWebsiteIdAsync (WebsiteId websiteId) {
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<Website>> ReadWebsitesByAccountIdAsync (AccountId accountId) {
		throw new NotImplementedException();
	}
}