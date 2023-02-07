using Neobyte.Cms.Backend.Core.Ports.Identity;

namespace Neobyte.Cms.Backend.Core.Identity.Managers; 

public class IdentityAuthorizationManager {

	private readonly IIdentityAuthorizationProvider _identityAuthorizationProvider;

	public IdentityAuthorizationManager (IIdentityAuthorizationProvider identityAuthorizationProvider) {
		_identityAuthorizationProvider = identityAuthorizationProvider; ;
	}

}