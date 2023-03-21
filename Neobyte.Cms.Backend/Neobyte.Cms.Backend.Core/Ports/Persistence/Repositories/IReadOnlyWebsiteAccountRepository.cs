using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IReadOnlyWebsiteAccountRepository {

	public Task<IEnumerable<Account>> ReadAccountsByWebsiteIdAsync (WebsiteId websiteId);

	public Task<IEnumerable<Website>> ReadWebsitesByAccountIdAsync (AccountId accountId);

	public Task<WebsiteAccount?> ReadWebsiteAccountByWebsiteIdAndAccountIdAsync (WebsiteId websiteId, AccountId accountId);

	public Task<IEnumerable<Account>> ReadUnassignedAccountsByWebsiteIdAsync (WebsiteId websiteId);

	public Task<IEnumerable<WebsiteAccount>> ReadAllWebsiteAccountsAsync (WebsiteId websiteId);

	public Task<IEnumerable<Website>> ReadUnassignedWebsitesByAccountIdAsync (AccountId accountId);
}