using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoreCSharp.Extensions.System.Collections.Generic;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Configuration;
using Neobyte.Cms.Backend.Identity.Repositories;
using Neobyte.Cms.Backend.Persistence.Entities.Accounts;
using Neobyte.Cms.Backend.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Identity.Adapters;

public class IdentityAuthenticationProvider : IIdentityAuthenticationProvider {

	private readonly SignInManager<IdentityAccountEntity> _signInManager;
	private readonly UserManager<IdentityAccountEntity> _userManager;
	private readonly SigningCredentials _credentials;
	private readonly JwtOptions _jwtOptions;
	private readonly JwtSecurityTokenHandler _tokenHandler;
	private readonly IUserStore<IdentityAccountEntity> _userStore;
	private readonly IUserEmailStore<IdentityAccountEntity> _emailStore;
	private readonly IdentityAccountRepository _identityAccountRepository;

	public IdentityAuthenticationProvider (SignInManager<IdentityAccountEntity> signInManager, IOptions<JwtOptions> jwtOptions, SigningCredentials credentials, UserManager<IdentityAccountEntity> userManager, JwtSecurityTokenHandler tokenHandler, IUserStore<IdentityAccountEntity> userStore, IdentityAccountRepository identityAccountRepository) {
		_signInManager = signInManager;
		_userManager = userManager;
		_credentials = credentials;
		_jwtOptions = jwtOptions.Value;
		_tokenHandler = tokenHandler;
		_userStore = userStore;
		_identityAccountRepository = identityAccountRepository;
		_emailStore = (IUserEmailStore<IdentityAccountEntity>)userStore;
	}

	public async Task<AccountsCreateResponseModel> CreateIdentityAccountAsync (Account account, string password) {
		var accountEntity = new AccountEntity(account.Id, account.Username, account.Bio, account.Enabled, account.CreationDate);
		var identityAccount = new IdentityAccountEntity { Account = accountEntity };

		await _userStore.SetUserNameAsync(identityAccount, identityAccount.Id.ToString(), CancellationToken.None);
		await _emailStore.SetEmailAsync(identityAccount, account.Email, CancellationToken.None);

		var result = await _userManager.CreateAsync(identityAccount, password);
		if (!result.Succeeded)
			return new AccountsCreateResponseModel(false) { Errors = result.Errors.Select(e => e.Description).ToArray() };

		await _userManager.AddToRolesAsync(identityAccount, account.Roles);

		return new AccountsCreateResponseModel(true) { AccountId = account.Id };
	}

	public async Task<bool> LoginAsync (string email, string password) {
		string normalizedEmail = NormalizeEmail(email);
		var identityAccount = await _identityAccountRepository.ReadIdentityAccountByEmailAsync(normalizedEmail);
		if (identityAccount is null)
			return false;
		if (!identityAccount.Account!.Enabled)
			return false;
		var signInResult = await _signInManager.PasswordSignInAsync(identityAccount, password, false, false);
		return signInResult.Succeeded;

	}

	public async Task<(string token, long expires)> GenerateJwtTokenAsync (AccountId accountId, bool rememberMe) {

		long expirationMilliseconds = rememberMe ? _jwtOptions.ExpirationLong : _jwtOptions.ExpirationShort;

		var identityAccount = await _identityAccountRepository.ReadIdentityAccountByAccountIdAsync(accountId);
		var roles = await _userManager.GetRolesAsync(identityAccount);

		var claims = new List<Claim>();
		roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
		claims.Add(new Claim(ClaimTypes.PrimarySid, identityAccount.Account!.Id.ToString()));

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

	public async Task<(bool valid, string[]? errors)> ChangePasswordAsync (AccountId accountId, string currentPassword, string newPassword) {
		var identityAccount = await _identityAccountRepository.ReadIdentityAccountByAccountIdAsync(accountId);
		var result = await _userManager.ChangePasswordAsync(identityAccount, currentPassword, newPassword);
		
		if (!result.Succeeded)
			return (false, new string[] { "Incorrect Password" });
		
		return (true, result.Errors.Select(e => e.Description).ToArray());
	}
	
	public async Task<(bool valid, string[]? errors)> ResetPasswordAsync (string email, string token, string newPassword) {
		var identityAccount = await _identityAccountRepository.ReadIdentityAccountByEmailAsync(email);
		if (identityAccount is null)
			return (false, new string[] { "Invalid Email" });
		var result = await _userManager.ResetPasswordAsync(identityAccount, token, newPassword);
		if (!result.Succeeded)
			return (false, result.Errors.Select(e => e.Description).ToArray());
		
		return (true, result.Errors.Select(e => e.Description).ToArray());
	}

	public async Task<AccountsGeneratePasswordResetTokenResponseModel> GeneratePasswordResetTokenAsync (AccountId accountId) {
		var identityAccount = await _identityAccountRepository.ReadIdentityAccountByAccountIdAsync(accountId);
		var token = await _userManager.GeneratePasswordResetTokenAsync(identityAccount);
		return new AccountsGeneratePasswordResetTokenResponseModel(true) { Token = token };
	}

	public string GenerateRandomPassword () {
		Random r = new Random();
		string lower = "abcdefghijklmnopqrstuvwxyz".Shuffle()[..4];
		string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Shuffle()[..4];
		string numbers = "0123456789".Shuffle()[..4];
		string special = "!@#$%^&*()_+".Shuffle()[..4];
		string password = lower + upper + numbers + special;
		return password.Shuffle();
	}
}