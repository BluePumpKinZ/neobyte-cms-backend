using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Configuration;
using Neobyte.Cms.Backend.Identity.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Identity.Adapters;

public class IdentityAuthorizationProvider : IIdentityAuthorizationProvider {

	private readonly JwtOptions _jwtOptions;
	private readonly JwtSecurityTokenHandler _tokenHandler;
	private readonly SymmetricSecurityKey _key;
	private readonly IdentityAccountRepository _identityAccountRepository;

	public IdentityAuthorizationProvider (IOptions<JwtOptions> optjwtOns, JwtSecurityTokenHandler tokenHandler, SymmetricSecurityKey key, IdentityAccountRepository identityAccountRepository) {
		_jwtOptions = optjwtOns.Value;
		_tokenHandler = tokenHandler;
		_key = key;
		_identityAccountRepository = identityAccountRepository;
	}

	public async Task<TokenValidationResult> ValidateTokenAsync (string token) {
		var validationResult = await _tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters {
			IssuerSigningKey = _key,
			ValidateIssuerSigningKey = true,
			ValidateIssuer = true,
			ValidIssuer = _jwtOptions.Issuer,
			ValidateAudience = true,
			ValidAudience = _jwtOptions.Audience,
			ClockSkew = TimeSpan.Zero
		});

		return validationResult;
	}

	public async Task<bool> CanLoginAsync (AccountId accountId) {
		var accountEntity = await _identityAccountRepository.ReadAccountByIdAsync(accountId);
		return accountEntity.Enabled;
	}

}