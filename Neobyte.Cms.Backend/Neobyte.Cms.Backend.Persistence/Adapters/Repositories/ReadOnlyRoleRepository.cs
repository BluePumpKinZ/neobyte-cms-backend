using Microsoft.EntityFrameworkCore;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Persistence.EF;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

public class ReadOnlyRoleRepository : IReadOnlyRoleRepository {

	private readonly EFDbContext _ctx;

	public ReadOnlyRoleRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<Role?> ReadRoleByName (string roleName) {
		return await _ctx.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
	}

}