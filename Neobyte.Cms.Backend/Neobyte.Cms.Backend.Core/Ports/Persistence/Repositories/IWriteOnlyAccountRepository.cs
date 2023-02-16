using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IWriteOnlyAccountRepository {

	public Task<Account> UpdateAccountAsync (Account account);

	public Task DeleteAccountByIdAsync (AccountId accountId);

}