using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoreCSharp.Extensions.System.Collections.Generic;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Identity.Adapters; 

public class IdentityAuthenticationProvider : IIdentityAuthenticationProvider {

	private readonly SignInManager<IdentityAccount> _signInManager;
	private readonly UserManager<IdentityAccount> _userManager;
	private readonly SigningCredentials _credentials;
	private readonly JwtOptions _jwtOptions;
	private readonly JwtSecurityTokenHandler _tokenHandler;

	public IdentityAuthenticationProvider (SignInManager<IdentityAccount> signInManager, IOptions<JwtOptions> jwtOptions, SigningCredentials credentials, UserManager<IdentityAccount> userManager, JwtSecurityTokenHandler tokenHandler) {
		_signInManager = signInManager;
		_jwtOptions = jwtOptions.Value;
		_credentials = credentials;
		_userManager = userManager;
		_tokenHandler = tokenHandler;
	}

	public async Task<bool> LoginAsync (string email, string password) {
		var signInResult = await _signInManager.PasswordSignInAsync(email, password, false, false);
		return signInResult.Succeeded;
	}

	public async Task<string> GenerateJwtTokenAsync (IdentityAccount identityAccount, bool rememberMe) {

		long expirationMilliseconds = rememberMe ? _jwtOptions.ExpirationLong : _jwtOptions.ExpirationShort;
		var roles = await _userManager.GetRolesAsync(identityAccount);

		var claims = new List<Claim>();
		roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
		claims.Add(new Claim(ClaimTypes.PrimarySid, identityAccount.Account!.Id.ToString()));
		claims.Add(new Claim(ClaimTypes.UserData, identityAccount.Id.ToString()));
		
		var tokenDescriptor = new SecurityTokenDescriptor {
			Subject = new ClaimsIdentity (claims.ToArray()),
			Issuer = _jwtOptions.Issuer,
			Audience = _jwtOptions.Audience,
			Expires = DateTime.UtcNow.AddMilliseconds(expirationMilliseconds),
			SigningCredentials = _credentials
		};

		var token = _tokenHandler.CreateToken(tokenDescriptor);
		return _tokenHandler.WriteToken(token);
	}

	public string NormalizeEmail (string email) {
		return _userManager.NormalizeEmail(email);
	}

}