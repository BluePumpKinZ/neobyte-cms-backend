using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Authentication.Principals;
using Neobyte.Cms.Backend.Identity.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Identity.Authentication;

public class JwtManager<TUser, TId, TPrincipal> where TPrincipal : IPrincipal<TId> {

	private readonly JwtSecurityTokenHandler _tokenHandler;
	private readonly JwtOptions _options;
	private readonly SymmetricSecurityKey _key;
	private readonly IPrincipalConverter<TUser, TId, TPrincipal> _principalConverter;

	public JwtManager (JwtSecurityTokenHandler tokenHandler, IOptions<IdentityOptions> options, IPrincipalConverter<TUser, TId, TPrincipal> principalConverter) {
		_tokenHandler = tokenHandler;
		_principalConverter = principalConverter;
		_options = options.Value.Jwt;
		_key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Secret));
	}

	public string GenerateToken (TPrincipal principal, long expirationMilliseconds) {

		var tokenDescriptor = new SecurityTokenDescriptor {
			Subject = new ClaimsIdentity(new Claim[] {
				new Claim("id", principal.Id!.ToString()!),
				new Claim("roles", string.Join (':', principal.Roles))
			}),
			Issuer = _options.Issuer,
			Audience = _options.Audience,
			Expires = DateTime.UtcNow.AddMilliseconds(expirationMilliseconds),
			SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
		};

		var token = _tokenHandler.CreateToken(tokenDescriptor);
		return _tokenHandler.WriteToken(token);
	}

	public async Task<(bool valid, TPrincipal? principal)> ValidateToken(string token) {

		var validationResult = await _tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters {
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = _key,
			ValidateIssuer = true,
			ValidIssuer = _options.Issuer,
			ValidateAudience = true,
			ValidAudience = _options.Audience,
			ClockSkew = TimeSpan.Zero
		});

		return _principalConverter.FromTokenValidationResult(validationResult);

	}

}