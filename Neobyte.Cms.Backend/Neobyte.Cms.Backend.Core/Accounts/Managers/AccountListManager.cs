using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Accounts.Managers; 

public class AccountListManager {

	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;

	public AccountListManager (IReadOnlyAccountRepository readOnlyAccountRepository) {
		_readOnlyAccountRepository = readOnlyAccountRepository;
	}

	public async Task<IEnumerable<Account>> GetAllAccountsAsync () {
		return await _readOnlyAccountRepository.ReadAllAccountsAsync();
	}

}