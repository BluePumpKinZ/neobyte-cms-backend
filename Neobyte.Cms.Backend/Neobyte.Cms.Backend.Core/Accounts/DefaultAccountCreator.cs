using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Configuration;

namespace Neobyte.Cms.Backend.Core.Accounts;

internal class DefaultAccountCreator {

	private readonly DefaultAccountOptions _options;
	private readonly AccountManager _accountManager;

	public DefaultAccountCreator (IOptions<CoreOptions> options, AccountManager accountManager) {
		_accountManager = accountManager;
		_options = options.Value.DefaultAccount;
	}

	public void CreateDefaultAccount () {

		if (!_options.AddOnAccountsEmpty)
			return;

		

	}

}