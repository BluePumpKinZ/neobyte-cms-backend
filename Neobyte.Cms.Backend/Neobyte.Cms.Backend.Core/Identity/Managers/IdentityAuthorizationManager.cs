using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Identity.Managers; 

public class IdentityAuthorizationManager {

	private readonly IReadOnlyRoleRepository _readOnlyRoleRepository;
	private readonly IWriteOnlyRoleRepository _writeOnlyRoleRepository;
	private readonly IIdentityAuthorizationProvider _authProvider;

	public IdentityAuthorizationManager (IReadOnlyRoleRepository readOnlyRoleRepository, IWriteOnlyRoleRepository writeOnlyRoleRepository, IIdentityAuthorizationProvider authProvider) {
		_readOnlyRoleRepository = readOnlyRoleRepository;
		_writeOnlyRoleRepository = writeOnlyRoleRepository;
		_authProvider = authProvider;
	}

	public async Task<Role?> GetRoleByName (string roleName) {
		return await _readOnlyRoleRepository.ReadRoleByName(roleName);
	}

	public async Task<Role> AddRole (Role role) {
		return await _writeOnlyRoleRepository.CreateRoleAsync(role);
	}

	public bool IsAuthorized (string policyName, string[] roles) {
		return _authProvider.IsAuthorized(policyName, roles);
	}

}