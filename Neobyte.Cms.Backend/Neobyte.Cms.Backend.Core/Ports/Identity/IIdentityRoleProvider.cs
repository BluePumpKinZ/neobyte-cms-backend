using Neobyte.Cms.Backend.Core.Identity;
using System;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Identity; 

public interface IIdentityRoleProvider {

	public Task AddRoleToIdentityUserAsync (Guid identityAccountId, Role role);

}