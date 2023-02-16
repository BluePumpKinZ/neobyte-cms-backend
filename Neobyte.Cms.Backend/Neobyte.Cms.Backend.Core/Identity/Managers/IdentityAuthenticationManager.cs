using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using Neobyte.Cms.Backend.Core.Ports.Identity;

namespace Neobyte.Cms.Backend.Core.Identity.Managers; 

public class IdentityAuthenticationManager {

	private readonly AccountManager _accountManager;
	private readonly IIdentityAuthenticationProvider _authenticationProvider;

	public IdentityAuthenticationManager (AccountManager accountManager, IIdentityAuthenticationProvider authenticationProvider) {
		_accountManager = accountManager;
		_authenticationProvider = authenticationProvider;
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

}