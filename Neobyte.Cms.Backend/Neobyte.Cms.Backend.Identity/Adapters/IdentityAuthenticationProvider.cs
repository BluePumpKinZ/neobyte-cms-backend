using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoreCSharp.Extensions.System.Collections.Generic;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Identity.Adapters;

public class IdentityAuthenticationProvider : IIdentityAuthenticationProvider {

	private readonly SignInManager<IdentityAccount> _signInManager;
	private readonly UserManager<IdentityAccount> _userManager;
	private readonly SigningCredentials _credentials;
	private readonly JwtOptions _jwtOptions;
	private readonly JwtSecurityTokenHandler _tokenHandler;
	private readonly IUserStore<IdentityAccount> _userStore;
	private readonly IUserEmailStore<IdentityAccount> _emailStore;
	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;

	public IdentityAuthenticationProvider (SignInManager<IdentityAccount> signInManager, IOptions<JwtOptions> jwtOptions, SigningCredentials credentials, UserManager<IdentityAccount> userManager, JwtSecurityTokenHandler tokenHandler, IUserStore<IdentityAccount> userStore, IReadOnlyAccountRepository readOnlyAccountRepository) {
		_signInManager = signInManager;
		_jwtOptions = jwtOptions.Value;
		_credentials = credentials;
		_userManager = userManager;
		_tokenHandler = tokenHandler;
		_userStore = userStore;
		_readOnlyAccountRepository = readOnlyAccountRepository;
		_emailStore = (IUserEmailStore<IdentityAccount>)userStore;
	}

	public async Task<AccountsCreateResponseModel> CreateIdentityAccountAsync (Account account, string email, string password) {
		var identityAccount = new IdentityAccount { Account = account };

		await _userStore.SetUserNameAsync(identityAccount, identityAccount.Id.ToString(), CancellationToken.None);
		await _emailStore.SetEmailAsync(identityAccount, email, CancellationToken.None);

		var result = await _userManager.CreateAsync(identityAccount, password);
		if (!result.Succeeded)
			return new AccountsCreateResponseModel(false) { Errors = result.Errors.Select(e => e.Description).ToArray() };

		return new AccountsCreateResponseModel(true) { AccountId = account.Id, IdentityAccountId = identityAccount.Id };
	}

	public async Task<bool> LoginAsync (string email, string password) {
		string normalizedEmail = NormalizeEmail(email);
		var identityAccount = await _readOnlyAccountRepository.ReadIdentityAccountByEmailAsync(normalizedEmail);
		if (identityAccount is null)
			return false;
		var signInResult = await _signInManager.PasswordSignInAsync(identityAccount, password, false, false);
		return signInResult.Succeeded;

	}

	public async Task<(string token, long expires)> GenerateJwtTokenAsync (IdentityAccount identityAccount, bool rememberMe) {

		long expirationMilliseconds = rememberMe ? _jwtOptions.ExpirationLong : _jwtOptions.ExpirationShort;
		var roles = await _userManager.GetRolesAsync(identityAccount);

		var claims = new List<Claim>();
		roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
		claims.Add(new Claim(ClaimTypes.PrimarySid, identityAccount.Account!.Id.ToString()));
		claims.Add(new Claim(ClaimTypes.UserData, identityAccount.Id.ToString()));

		var tokenDescriptor = new SecurityTokenDescriptor {
			Subject = new ClaimsIdentity(claims.ToArray()),
			Issuer = _jwtOptions.Issuer,
			Audience = _jwtOptions.Audience,
			Expires = DateTime.UtcNow.AddMilliseconds(expirationMilliseconds),
			SigningCredentials = _credentials
		};

		var token = _tokenHandler.CreateToken(tokenDescriptor);
		return (_tokenHandler.WriteToken(token), expirationMilliseconds);
	}

	public string NormalizeEmail (string email) {
		return _userManager.NormalizeEmail(email);
	}

}