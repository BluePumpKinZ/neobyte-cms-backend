using Microsoft.Extensions.Logging;
using Neobyte.Cms.Backend.Identity.Authorization.Policies;
using System.Linq;

namespace Neobyte.Cms.Backend.Identity.Authorization;

public class AuthorizationManager {

	private readonly PolicyStore _policyStore;
	private readonly ILogger<AuthorizationManager> _logger;

	public AuthorizationManager (PolicyStore policyStore, ILogger<AuthorizationManager> logger) {
		_policyStore = policyStore;
		_logger = logger;
	}

	public bool IsAuthorized (string policyName, string[] roles) {
		if (!_policyStore.Policies.TryGetValue(policyName, out var allowedRoles)) {
			_logger.LogWarning("Policy {PolicyName} not found. Has it been added to the policy store?", policyName);
			return false;
		}

		return roles.Any(r => allowedRoles.Any(ar => ar.Name == r));
	}

	public void AddPolicy (string policyName, PolicyRole[] roles) {
		_policyStore.Policies.Add(policyName, roles);
	}

	public void RemovePolicy (string policyName) {
		_policyStore.Policies.Remove(policyName);
	}

}