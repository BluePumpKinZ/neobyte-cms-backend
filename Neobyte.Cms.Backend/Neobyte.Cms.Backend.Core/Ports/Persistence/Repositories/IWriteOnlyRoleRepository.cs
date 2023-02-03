using Neobyte.Cms.Backend.Domain.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IWriteOnlyRoleRepository {

	public Task<Role> CreateRoleAsync (Role role);

}