using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
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

	public async Task<IdentityAccount> GetIdentityAccountWithAccountByEmail (string normalizedEmail) {
		return await _readOnlyAccountRepository.ReadIdentityAccountWithAccountByEmail(normalizedEmail);
	}

	public async Task<bool> GetOwnerAccountExistsAsync () {
		return await _readOnlyAccountRepository.ReadOwnerAccountExistsAsync();
	}

}