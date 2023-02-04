using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Accounts.Managers;

public class ClientAccountManager {

	private readonly AccountManager _accountManager;
	private readonly IIdentityAuthenticationProvider _identityAuthenticationProvider;

	public ClientAccountManager (AccountManager accountManager, IIdentityAuthenticationProvider identityAuthenticationProvider) {
		_accountManager = accountManager;
		_identityAuthenticationProvider = identityAuthenticationProvider;
	}

	public async Task<AccountsClientCreateResponseModel> CreateClientAccountAsync (AccountsClientCreateRequestModel request) {
		var account = new Account(request.Email, request.FirstName, request.LastName);
		var passwordUpdateResult = await _identityAuthenticationProvider.UpdateAccountPasswordAsync(account, request.Password);

		// if (!passwordUpdateResult.valid)

		return new AccountsClientCreateResponseModel { Account = await _accountManager.AddAccountAsync(account) };
	}

}