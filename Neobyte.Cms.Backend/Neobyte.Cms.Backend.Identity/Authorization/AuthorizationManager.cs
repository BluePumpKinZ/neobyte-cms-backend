using Neobyte.Cms.Backend.Identity.Authorization.Policies;
using System.Linq;

namespace Neobyte.Cms.Backend.Identity.Authorization;

public class AuthorizationManager {

	private readonly PolicyStore _policyStore;

	public AuthorizationManager (PolicyStore policyStore) {
		_policyStore = policyStore;
	}

	public bool IsAuthorized (string policyName, string role) {
		if (!_policyStore.Policies.TryGetValue(policyName, out var allowedRoles)) {
			return false;
		}

		return allowedRoles.Any(r => r.Name == role);
	}

	public void AddPolicy (string policyName, PolicyRole[] roles) {
		_policyStore.Policies.Add(policyName, roles);
	}

	public void RemovePolicy (string policyName) {
		_policyStore.Policies.Remove(policyName);
	}

}
