using Microsoft.AspNetCore.Http;
using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using System;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Identity.Managers;

public class IdentityAuthenticationManager {

	private readonly IIdentityAuthenticationProvider _identityAuthenticationProvider;
	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;

	public IdentityAuthenticationManager (IIdentityAuthenticationProvider identityAuthenticationProvider, IReadOnlyAccountRepository readOnlyAccountRepository) {
		_identityAuthenticationProvider = identityAuthenticationProvider;
		_readOnlyAccountRepository = readOnlyAccountRepository;
	}

	public async Task<IdentityRegisterResponseModel> Register (IdentityRegisterRequestModel request) {
		var result = await _identityAuthenticationProvider.RegisterAsync(request);
		var response = new IdentityRegisterResponseModel(result.result, result.errors);
		throw new NotImplementedException();
	}

	public async Task<IdentityLoginResponseModel> Login (IdentityLoginRequestModel request) {
		var result = await _identityAuthenticationProvider.LoginAsync(request);
		if (result != IdentityLoginResponseModel.LoginResult.Success) {
			return new IdentityLoginResponseModel {
				Result = result
			};
		}

		var account = await _readOnlyAccountRepository.ReadAccountByEmailWithRolesAsync(request.Email);
		var jwtToken = _identityAuthenticationProvider.GenerateTokenForAccount(account!, request.RememberMe);
		return new IdentityLoginResponseModel {
			Result = IdentityLoginResponseModel.LoginResult.Success,
			JwtToken = jwtToken
		};
	}

	public async Task<IdentityAuthenticateResponseModel> Authenticate (HttpContext httpContext) {
		return await _identityAuthenticationProvider.Authenticate(httpContext);
	}

}