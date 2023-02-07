using Neobyte.Cms.Backend.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IReadOnlyAccountRepository {

	public Task<IdentityAccount> ReadIdentityAccountWithAccountByEmail (string normalizedEmail);

	public Task<bool> ReadOwnerAccountExistsAsync ();

	public Task<IdentityAccount> ReadByIdentityAccountIdAsync (Guid identityAccountId);

	public Task<IdentityAccount?> ReadIdentityAccountByEmailAsync (string normalizedEmail);

	public Task<IEnumerable<Account>> ReadAllAccountsAsync ();

	public Task<Account> ReadAccountById (AccountId accountId);

}