using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Core.Ports.Identity; 

public interface IIdentityRoleProvider {

	public Task UpdateRoles (Account account);

}