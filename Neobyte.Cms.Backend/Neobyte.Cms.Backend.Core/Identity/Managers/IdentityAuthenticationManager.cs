using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Identity.Managers;

public class IdentityAuthenticationManager {

	private readonly IIdentityAuthenticationProvider _identityLoginProvider;

	public IdentityAuthenticationManager (IIdentityAuthenticationProvider identityLoginProvider) {
		_identityLoginProvider = identityLoginProvider;
	}

	public async Task<IdentityLoginResponseModel> Login (IdentityLoginRequestModel request) {
		return await _identityLoginProvider.Login(request);
	}

}