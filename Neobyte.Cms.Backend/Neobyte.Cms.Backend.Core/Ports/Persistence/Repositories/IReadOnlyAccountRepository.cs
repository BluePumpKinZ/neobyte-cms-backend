using Neobyte.Cms.Backend.Domain.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IReadOnlyAccountRepository {

	public Task<bool> ReadOwnerAccountExistsAsync ();

	public Task<Account?> ReadAccountByEmailAsync (string normalizedEmail);

	public Task<IEnumerable<Account>> ReadAllAccountsAsync ();

	public Task<Account?> ReadAccountByIdAsync (AccountId accountId);

}