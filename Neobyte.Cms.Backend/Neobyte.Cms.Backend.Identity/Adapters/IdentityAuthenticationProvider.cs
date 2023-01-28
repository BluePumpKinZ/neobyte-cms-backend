using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Neobyte.Cms.Backend.Core.Exceptions.Identity;
using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Identity.Adapters;

internal class IdentityAuthenticationProvider : IIdentityAuthenticationProvider {

	private readonly UserManager<AccountIdentityUser> _userManager;
	private readonly SignInManager<AccountIdentityUser> _signInManager;
	private readonly IUserStore<AccountIdentityUser> _userStore;
	private readonly IUserEmailStore<AccountIdentityUser> _emailStore;
	private readonly ILogger<IdentityAuthenticationProvider> _logger;

	public IdentityAuthenticationProvider (UserManager<AccountIdentityUser> userManager, SignInManager<AccountIdentityUser> signInManager, IUserStore<AccountIdentityUser> userStore, ILogger<IdentityAuthenticationProvider> logger) {
		_userManager = userManager;
		_signInManager = signInManager;
		_userStore = userStore;
		_emailStore = (IUserEmailStore<AccountIdentityUser>)userStore;
		_logger = logger;
	}

	public async Task<IdentityRegisterResponseModel> Register (IdentityRegisterRequestModel request) {
		var user = new AccountIdentityUser {
			Account = new Account(request.FirstName, request.LastName),
		};

		await _userStore.SetUserNameAsync(user, request.Email, CancellationToken.None);
		await _emailStore.SetEmailAsync(user, request.Email, CancellationToken.None);

		var registerResult = await _userManager.CreateAsync(user, request.Password);
		if (!registerResult.Succeeded)
			return new IdentityRegisterResponseModel(
				IdentityRegisterResponseModel.RegisterResult.Failed,
				registerResult.Errors.Select(e => e.Description)
			);

		string userId = await _userManager.GetUserIdAsync(user);
		string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

		// TODO: send email to confirm account

		return new IdentityRegisterResponseModel(IdentityRegisterResponseModel.RegisterResult.Success);
	}

	public async Task<IdentityLoginResponseModel> Login (IdentityLoginRequestModel request) {
		try {
			var signInResult = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, false);
			if (signInResult.Succeeded) return new IdentityLoginResponseModel {
				Result = IdentityLoginResponseModel.LoginResult.Success
			};

			if (signInResult.RequiresTwoFactor) return new IdentityLoginResponseModel {
				Result = IdentityLoginResponseModel.LoginResult.RequiresTwoFactor
			};

			if (signInResult.IsLockedOut) return new IdentityLoginResponseModel {
				Result = IdentityLoginResponseModel.LoginResult.LockedOut
			};

			return new IdentityLoginResponseModel {
				Result = IdentityLoginResponseModel.LoginResult.BadCredentials
			};
		} catch (Exception e) {
			_logger.LogError("Error while logging in {error}", e);
			throw new IdentityLoginException("Error while logging in", e);
		}
	}

	public async Task Logout () {
		await _signInManager.SignOutAsync();
	}


}