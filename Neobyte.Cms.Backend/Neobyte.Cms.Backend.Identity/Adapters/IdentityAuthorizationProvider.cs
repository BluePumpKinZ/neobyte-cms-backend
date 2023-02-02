using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Identity.Authorization;

namespace Neobyte.Cms.Backend.Identity.Adapters;

internal class IdentityAuthorizationProvider : IIdentityAuthorizationProvider {

	private readonly AuthorizationManager _identityAuthorizationManager;

	public IdentityAuthorizationProvider (AuthorizationManager identityAuthorizationManager) {
		_identityAuthorizationManager = identityAuthorizationManager;
	}

	public bool IsAuthorized (string policyName, string[] roles) {
		return _identityAuthorizationManager.IsAuthorized(policyName, roles);
	}

}