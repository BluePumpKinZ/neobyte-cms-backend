using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Identity.Managers;

public class IdentityAuthenticationManager {

	private readonly IIdentityAuthenticationProvider _identityLoginProvider;
	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;

	public IdentityAuthenticationManager (IIdentityAuthenticationProvider identityLoginProvider, IReadOnlyAccountRepository readOnlyAccountRepository) {
		_identityLoginProvider = identityLoginProvider;
		_readOnlyAccountRepository = readOnlyAccountRepository;
	}

	public async Task<IdentityRegisterResponseModel> Register (IdentityRegisterRequestModel request) {
		var result = await _identityLoginProvider.RegisterAsync(request);
		var response = new IdentityRegisterResponseModel(result.result, result.errors);
		return null;
	}

	public async Task<IdentityLoginResponseModel> Login (IdentityLoginRequestModel request) {
		var result = await _identityLoginProvider.LoginAsync(request);
		if (result != IdentityLoginResponseModel.LoginResult.Success) {
			return new IdentityLoginResponseModel {
				Result = result
			};
		}

		var account = await _readOnlyAccountRepository.ReadAccountByEmailWithRolesAsync(request.Email);
		var jwtToken = _identityLoginProvider.GenerateTokenForAccount(account!, request.RememberMe);
		return new IdentityLoginResponseModel {
			Result = IdentityLoginResponseModel.LoginResult.Success,
			JwtToken = jwtToken
		};
	}

}