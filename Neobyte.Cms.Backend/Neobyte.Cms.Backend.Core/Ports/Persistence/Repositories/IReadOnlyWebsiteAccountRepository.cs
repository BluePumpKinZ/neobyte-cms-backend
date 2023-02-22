﻿using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IReadOnlyWebsiteAccountRepository {

	public Task<IEnumerable<Account>> ReadAccountsByWebsiteIdAsync (WebsiteId websiteId);
	public Task<IEnumerable<Website>> ReadWebsitesByAccountIdAsync (AccountId accountId);
}