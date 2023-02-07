using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using System;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Accounts.Managers;

public class AccountManager {

	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;
	private readonly IIdentityAuthenticationProvider _identityAuthenticationProvider;
	private readonly IIdentityRoleProvider _identityRoleProvider;

	public AccountManager (IReadOnlyAccountRepository readOnlyAccountRepository, IIdentityAuthenticationProvider identityAuthenticationProvider, IIdentityRoleProvider identityRoleProvider) {
		_readOnlyAccountRepository = readOnlyAccountRepository;
		_identityAuthenticationProvider = identityAuthenticationProvider;
		_identityRoleProvider = identityRoleProvider;
	}

	public async Task<AccountsCreateResponseModel> CreateAccountAsync (AccountsCreateRequestModel request, Role role) {
		var account = new Account(request.FirstName, request.LastName);
		var response = await _identityAuthenticationProvider.CreateIdentityAccountAsync(account, request.Email, request.Password);
		if (response.Success)
			await _identityRoleProvider.AddRoleToIdentityUserAsync(response.IdentityAccountId!.Value, role);
		return response;
	}

	public async Task<Account> GetAccountDetails (AccountId accountId) {
		return await _readOnlyAccountRepository.ReadAccountById(accountId);
	}

	public async Task<IdentityAccount> GetIdentityAccountWithAccountByEmail (string normalizedEmail) {
		return await _readOnlyAccountRepository.ReadIdentityAccountWithAccountByEmail(normalizedEmail);
	}

	public async Task<bool> GetOwnerAccountExistsAsync () {
		return await _readOnlyAccountRepository.ReadOwnerAccountExistsAsync();
	}

	public async Task<AccountChangePasswordResponseModel> ChangePasswordAsync (AccountChangePasswordRequestModel request, Guid identityAccountId) {
		var result = await _identityAuthenticationProvider.ChangePasswordAsync(identityAccountId, request.OldPassword, request.NewPassword);
		return new AccountChangePasswordResponseModel(result.valid, result.errors);
	}

	public async Task ChangeDetailsAsync (AccountChangeDetailsRequestModel request, AccountId accountId) {
		var account = await _readOnlyAccountRepository.ReadAccountById(accountId);
		account.Firstname = request.FirstName;
		account.Lastname = request.LastName;
	}

	public async Task<bool> ChangeEmailAsync (string email, Guid identityAccountId) {
		return await _identityAuthenticationProvider.ChangeEmailAsync(identityAccountId, email);
	}

}