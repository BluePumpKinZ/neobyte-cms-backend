using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Authentication;
using Neobyte.Cms.Backend.Identity.Authentication.Principals;
using Neobyte.Cms.Backend.Identity.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Identity.Adapters;

internal class IdentityAuthenticationProvider : IIdentityAuthenticationProvider {

	private readonly AuthenticationManager _authenticationManager;
	private readonly JwtManager<Account, AccountId, AccountPrincipal> _jwtManager;
	private readonly JwtOptions _jwtOptions;
	private readonly IPrincipalConverter<Account, AccountId, AccountPrincipal> _principalConverter;

	public IdentityAuthenticationProvider (AuthenticationManager authenticationManager, JwtManager<Account, AccountId, AccountPrincipal> jwtManager, IPrincipalConverter<Account, AccountId, AccountPrincipal> principalConverter, IOptions<IdentityOptions> options) {
		_authenticationManager = authenticationManager;
		_jwtManager = jwtManager;
		_principalConverter = principalConverter;
		_jwtOptions = options.Value.Jwt;
	}

	public async Task<(IdentityRegisterResponseModel.RegisterResult result, IEnumerable<string> errors)> RegisterAsync (IdentityRegisterRequestModel request) {
		throw new NotImplementedException();
	}

	public async Task<IdentityLoginResponseModel.LoginResult> LoginAsync (IdentityLoginRequestModel request) {
		var result = await _authenticationManager.LoginAsync(request);
		return result switch {
				LoginResult.Success => IdentityLoginResponseModel.LoginResult.Success,
				LoginResult.InvalidCredentials => IdentityLoginResponseModel.LoginResult.InvalidCredentials,
				LoginResult.NotAllowed => IdentityLoginResponseModel.LoginResult.NotAllowed,
				_ => IdentityLoginResponseModel.LoginResult.NotAllowed
			};
	}

	public string GenerateTokenForAccount (Account accountWithRoles, bool rememberMe) {
		return GenerateTokenForAccount(accountWithRoles, rememberMe ? _jwtOptions.ExpirationLong : _jwtOptions.ExpirationShort);
	}

	public async Task<IdentityAuthenticateResponseModel> Authenticate (HttpContext httpContext) {
		var authHeaders = httpContext.Request.Headers["Authorization"];
		if (authHeaders.Count != 1)
			return IdentityAuthenticateResponseModel.Unauthenticated();

		var authHeader = authHeaders[0];
		if (!authHeader?.StartsWith("Bearer ") ?? true)
			return IdentityAuthenticateResponseModel.Unauthenticated();

		var token = authHeader!["Bearer ".Length..];
		var validationResult = await _jwtManager.ValidateTokenAsync(token);
		if (!validationResult.valid)
			return IdentityAuthenticateResponseModel.Unauthenticated();

		var principal = validationResult.principal!;
		return IdentityAuthenticateResponseModel.Authenticated(principal.Id, principal.Roles, true);
	}

	public string GenerateTokenForAccount (Account accountWithRoles, long expirationMilliseconds) {
		var principal = _principalConverter.FromUser(accountWithRoles);
		return _jwtManager.GenerateToken(principal, expirationMilliseconds);
	}

}