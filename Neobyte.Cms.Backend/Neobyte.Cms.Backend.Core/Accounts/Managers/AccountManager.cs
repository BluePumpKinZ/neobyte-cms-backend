using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Accounts.Managers;

public class AccountManager {

	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;

	public AccountManager (IReadOnlyAccountRepository readOnlyAccountRepository) {
		_readOnlyAccountRepository = readOnlyAccountRepository;
	}

	public async Task<Account> AddAccountAsync (Account account) {
		return await _readOnlyAccountRepository.CreateAccountAsync(account);
	}

	public async Task<IdentityAccount> GetIdentityAccountWithAccountByEmail (string normalizedEmail) {
		return await _readOnlyAccountRepository.ReadIdentityAccountWithAccountByEmail(normalizedEmail);
	}

}