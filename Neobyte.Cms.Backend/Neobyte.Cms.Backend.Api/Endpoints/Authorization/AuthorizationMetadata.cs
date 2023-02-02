using Neobyte.Cms.Backend.Core.Identity;

namespace Neobyte.Cms.Backend.Api.Endpoints.Authorization; 

public readonly struct AuthorizationMetadata {

	public string PolicyName { get; }

	public AuthorizationMetadata (Privileges privileges) {
		PolicyName = privileges.PrivilegeName;
	}

}