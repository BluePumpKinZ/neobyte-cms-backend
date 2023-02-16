using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Core.Identity.Managers; 

public class IdentityManager {

	private readonly AccountManager _accountManager;
	private readonly IIdentityAuthenticationProvider _authenticationProvider;
	private readonly IIdentityAuthorizationProvider _authorizationProvider;

	public IdentityManager (AccountManager accountManager, IIdentityAuthenticationProvider authenticationProvider, IIdentityAuthorizationProvider authorizationProvider) {
		_accountManager = accountManager;
		_authenticationProvider = authenticationProvider;
		_authorizationProvider = authorizationProvider;
	}

	public async Task<IdentityLoginResponseModel> LoginAsync (IdentityLoginRequestModel request) {
		var validLogin = await _authenticationProvider.LoginAsync(request.Email, request.Password);
		if (!validLogin)
			return new IdentityLoginResponseModel (false, null, null);

		var normalizedEmail = _authenticationProvider.NormalizeEmail(request.Email);
		var identityAccount = await _accountManager.GetIdentityAccountWithAccountByEmail(normalizedEmail);
		var jwtToken = await _authenticationProvider.GenerateJwtTokenAsync(identityAccount!, request.RememberMe);
		return new IdentityLoginResponseModel(true, jwtToken.token, jwtToken.expires);
	}

	public async Task<bool> CanLoginAsync (AccountId accountId) {
		return await _authorizationProvider.CanLoginAsync(accountId);
	}

}