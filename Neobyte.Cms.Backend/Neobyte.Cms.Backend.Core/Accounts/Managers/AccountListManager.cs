using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Core.Accounts.Managers; 

public class AccountListManager {

	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;
	private readonly IWriteOnlyAccountRepository _writeOnlyAccountRepository;

	public AccountListManager (IReadOnlyAccountRepository readOnlyAccountRepository, IWriteOnlyAccountRepository writeOnlyAccountRepository) {
		_readOnlyAccountRepository = readOnlyAccountRepository;
		_writeOnlyAccountRepository = writeOnlyAccountRepository;
	}

	public async Task<IEnumerable<Account>> GetAllAccountsAsync () {
		return await _readOnlyAccountRepository.ReadAllAccountsAsync();
	}

	public async Task DeleteAccountAsync (AccountId accountId) {
		await _writeOnlyAccountRepository.DeleteAccountByIdAsync(accountId);
	}

}