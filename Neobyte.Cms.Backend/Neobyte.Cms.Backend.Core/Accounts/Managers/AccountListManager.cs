using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Core.Accounts.Managers; 

public class AccountListManager {

	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;
	private readonly IWriteOnlyAccountRepository _writeOnlyAccountRepository;
	private readonly IIdentityRoleProvider _identityRoleProvider;

	public AccountListManager (IReadOnlyAccountRepository readOnlyAccountRepository, IWriteOnlyAccountRepository writeOnlyAccountRepository, IIdentityRoleProvider identityRoleProvider) {
		_readOnlyAccountRepository = readOnlyAccountRepository;
		_writeOnlyAccountRepository = writeOnlyAccountRepository;
		_identityRoleProvider = identityRoleProvider;
	}

	public async Task<IEnumerable<Account>> GetAllAccountsAsync () {
		return await _readOnlyAccountRepository.ReadAllAccountsAsync();
	}

	public async Task DeleteAccountAsync (AccountId accountId) {
		await _writeOnlyAccountRepository.DeleteAccountByIdAsync(accountId);
	}

	public async Task<Account> EditAccountDetailsAsync (AccountChangeDetailsOwnerRequestModel request) {
		var account = await _readOnlyAccountRepository.ReadAccountByIdAsync(request.AccountId);
		if (account is null)
			throw new AccountNotFoundException($"Account {request.AccountId} not found");
		
		account.Email = request.Email;
		account.Username = request.Username;
		account.Bio = request.Bio;
		account.Enabled = request.Enabled;
		account.Roles = request.Roles;
		
		var updatedAccount = await _writeOnlyAccountRepository.UpdateAccountAsync(account);
		await _identityRoleProvider.UpdateRoles(updatedAccount);
		return updatedAccount;
	}

}