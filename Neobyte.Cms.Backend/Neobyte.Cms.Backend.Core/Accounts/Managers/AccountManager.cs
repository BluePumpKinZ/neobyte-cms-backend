using Microsoft.AspNetCore.Identity.UI.Services;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Mailing;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using System;
using System.Linq;

namespace Neobyte.Cms.Backend.Core.Accounts.Managers;

public class AccountManager {
	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;
	private readonly IWriteOnlyAccountRepository _writeOnlyAccountRepository;
	private readonly IIdentityAuthenticationProvider _identityAuthenticationProvider;
	private readonly IMailingProvider _mailingProvider;

	public AccountManager (IReadOnlyAccountRepository readOnlyAccountRepository,
		IWriteOnlyAccountRepository writeOnlyAccountRepository,
		IIdentityAuthenticationProvider identityAuthenticationProvider, IMailingProvider mailingProvider) {
		_readOnlyAccountRepository = readOnlyAccountRepository;
		_writeOnlyAccountRepository = writeOnlyAccountRepository;
		_identityAuthenticationProvider = identityAuthenticationProvider;
		_mailingProvider = mailingProvider;
	}

	public async Task<AccountsCreateResponseModel> CreateAccountWithPasswordAsync (
		AccountsWithPasswordCreateRequestModel request) {
		Role role = Role.All.SingleOrDefault(r =>
			string.Equals(r.RoleName, request.Role, StringComparison.InvariantCultureIgnoreCase));
		if (role.RoleName is null) // check rolename because role will never be null because it is a struct
			return new AccountsCreateResponseModel(false)
				{Errors = new string[] {$"Role {request.Role} does not exist"}};
		var account = new Account(request.Email, request.Username, request.Bio, new string[] {role.RoleName});
		var accountResponse =
			await _identityAuthenticationProvider.CreateIdentityAccountAsync(account, request.Password);

		return accountResponse;
	}

	public async Task<AccountsCreateResponseModel> CreateAccountAsync (AccountsCreateRequestModel request) {
		string password = _identityAuthenticationProvider.GenerateRandomPassword();
		var model = new AccountsWithPasswordCreateRequestModel() {
			Email = request.Email, Bio = request.Bio, Username = request.Username, Role = request.Role,
			Password = password
		};
		return await CreateAccountWithPasswordAsync(model);
	}

	public async Task<Account> GetAccountDetailsAsync (AccountId accountId) {
		var account = await _readOnlyAccountRepository.ReadAccountByIdAsync(accountId)
		              ?? throw new AccountNotFoundException($"Account {accountId} not found");

		return account;
	}

	public async Task<Account?> GetAccountWithAccountByEmailAsync (string normalizedEmail) {
		return await _readOnlyAccountRepository.ReadAccountByEmailAsync(normalizedEmail);
	}

	public async Task<bool> GetOwnerAccountExistsAsync () {
		return await _readOnlyAccountRepository.ReadOwnerAccountExistsAsync();
	}

	public async Task<AccountChangePasswordResponseModel> ChangePasswordAsync (
		AccountChangePasswordRequestModel request, AccountId accountId) {
		var (valid, errors) =
			await _identityAuthenticationProvider.ChangePasswordAsync(accountId, request.OldPassword,
				request.NewPassword);
		return new AccountChangePasswordResponseModel(valid, errors);
	}

	public async Task<AccountResetPasswordResponseModel> ResetPasswordAsync (AccountResetPasswordRequestModel request) {
		var (valid, errors) =
			await _identityAuthenticationProvider.ResetPasswordAsync(request.Email, request.Token, request.Password);
		return new AccountResetPasswordResponseModel(valid, errors);
	}

	public async Task ChangeDetailsAsync (AccountChangeDetailsRequestModel request, AccountId accountId) {
		var account = await _readOnlyAccountRepository.ReadAccountByIdAsync(accountId)
		              ?? throw new AccountNotFoundException($"Account {accountId} not found");
		account.Email = request.Email;
		account.Username = request.Username;
		account.Bio = request.Bio;
		await _writeOnlyAccountRepository.UpdateAccountAsync(account);
	}
}