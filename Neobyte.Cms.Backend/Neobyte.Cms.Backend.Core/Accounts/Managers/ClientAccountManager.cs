using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;
using System;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Accounts.Managers;

public class ClientAccountManager {

	private readonly AccountManager _accountManager;
	private readonly IIdentityAuthenticationProvider _identityAuthenticationProvider;

	public ClientAccountManager (AccountManager accountManager, IIdentityAuthenticationProvider identityAuthenticationProvider) {
		_accountManager = accountManager;
		_identityAuthenticationProvider = identityAuthenticationProvider;
	}

	public async Task<Account> CreateClientAccountAsync (AccountsClientCreateRequestModel request) {
		throw new NotImplementedException();
	}

}