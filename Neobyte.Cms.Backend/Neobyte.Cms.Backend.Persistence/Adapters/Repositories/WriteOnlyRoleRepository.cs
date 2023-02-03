using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Persistence.EF;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

public class WriteOnlyRoleRepository : IWriteOnlyRoleRepository {

	private readonly EFDbContext _ctx;

	public WriteOnlyRoleRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<Role> CreateRoleAsync (Role role) {
		return (await _ctx.Roles.AddAsync(role)).Entity;
	}

}