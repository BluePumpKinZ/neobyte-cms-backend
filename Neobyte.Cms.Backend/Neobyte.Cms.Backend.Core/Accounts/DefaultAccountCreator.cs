using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoreCSharp.Extensions.System.Collections.Generic;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Configuration;
using Neobyte.Cms.Backend.Core.Identity;

namespace Neobyte.Cms.Backend.Core.Accounts;

internal class DefaultAccountCreator {

	private readonly DefaultAccountOptions _options;
	private readonly AccountManager _accountManager;
	private readonly ILogger<DefaultAccountCreator> _logger;

	public DefaultAccountCreator (IOptions<CoreOptions> options, AccountManager accountManager, ILogger<DefaultAccountCreator> logger) {
		_accountManager = accountManager;
		_logger = logger;
		_options = options.Value.DefaultAccount;
	}

	public async Task CreateDefaultAccount () {

		if (!_options.AddOnAccountsEmpty)
			return;

		if (await _accountManager.GetOwnerAccountExistsAsync())
			return;

		var request = new AccountsWithPasswordCreateRequestModel() {
			Username = _options.Username,
			Bio = _options.Bio,
			Email = _options.Email,
			Password = _options.Password,
			Role = Role.Owner.RoleName
		};

		var response = await _accountManager.CreateAccountWithPasswordAsync(request);
		if (response.Success) {
			_logger.LogInformation("Default owner account created");
			return;
		}

		response.Errors!.ForEach(e => _logger.LogError("Error creating default user: {Error}", e));
	}

}