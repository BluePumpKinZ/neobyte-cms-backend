using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Accounts.Managers;

public class AccountManager {

	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;
	private readonly IWriteOnlyAccountRepository _writeOnlyAccountRepository;
	private readonly IIdentityAuthenticationProvider _identityAuthenticationProvider;

	public AccountManager (IReadOnlyAccountRepository readOnlyAccountRepository, IWriteOnlyAccountRepository writeOnlyAccountRepository, IIdentityAuthenticationProvider identityAuthenticationProvider) {
		_readOnlyAccountRepository = readOnlyAccountRepository;
		_writeOnlyAccountRepository = writeOnlyAccountRepository;
		_identityAuthenticationProvider = identityAuthenticationProvider;
	}

	public async Task<AccountsCreateResponseModel> CreateAccountAsync (AccountsCreateRequestModel request) {
		Role role = Role.All.SingleOrDefault(r => string.Equals (r.RoleName, request.Role, StringComparison.InvariantCultureIgnoreCase));
		if (role.RoleName is null) // check rolename because role will never be null because it is a struct
			return new AccountsCreateResponseModel(false) { Errors = new string[] { $"Role {request.Role} does not exist" }};
		var account = new Account(request.Email, request.Username, request.Bio, new string[] { role.RoleName });
		return await _identityAuthenticationProvider.CreateIdentityAccountAsync(account, request.Password);
	}

	public async Task<Account> GetAccountDetails (AccountId accountId) {
		var account = await _readOnlyAccountRepository.ReadAccountByIdAsync(accountId);
		if (account is null)
			throw new AccountNotFoundException($"Account {accountId} not found");

		return account;
	}

	public async Task<Account?> GetIdentityAccountWithAccountByEmail (string normalizedEmail) {
		return await _readOnlyAccountRepository.ReadAccountByEmailAsync(normalizedEmail);
	}

	public async Task<bool> GetOwnerAccountExistsAsync () {
		return await _readOnlyAccountRepository.ReadOwnerAccountExistsAsync();
	}

	public async Task<AccountChangePasswordResponseModel> ChangePasswordAsync (AccountChangePasswordRequestModel request, AccountId accountId) {
		var result = await _identityAuthenticationProvider.ChangePasswordAsync(accountId, request.OldPassword, request.NewPassword);
		return new AccountChangePasswordResponseModel(result.valid, result.errors);
	}

	public async Task ChangeDetailsAsync (AccountChangeDetailsRequestModel request, AccountId accountId) {
		var account = await _readOnlyAccountRepository.ReadAccountByIdAsync(accountId);
		account.Email = request.Email;
		account.Username = request.Username;
		account.Bio = request.Bio;
		await _writeOnlyAccountRepository.UpdateAccountAsync(account);
	}

}