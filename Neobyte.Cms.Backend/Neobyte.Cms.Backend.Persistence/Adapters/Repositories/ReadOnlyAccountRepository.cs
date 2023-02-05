using Microsoft.EntityFrameworkCore;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Persistence.EF;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

public class ReadOnlyAccountRepository : IReadOnlyAccountRepository {

	private readonly EFDbContext _ctx;

	public ReadOnlyAccountRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<IdentityAccount> ReadIdentityAccountWithAccountByEmail (string normalizedEmail) {
		return await _ctx.Users
			.Include(u => u.Account)
			.Where(u => u.NormalizedEmail == normalizedEmail.ToUpper())
			.SingleAsync();
	}

	public async Task<bool> ReadOwnerAccountExistsAsync () {
		Guid ownerRoleId = (await _ctx.Roles.SingleAsync(r => r.Name == "Owner")).Id;
		return await _ctx.Users
			.Where(u => u.Account != null)
			.AnyAsync(u => _ctx.UserRoles
				.Where(ur => ur.UserId == u.Id)
				.Any(ur => ur.RoleId == ownerRoleId));
	}

	public async Task<IdentityAccount> ReadByIdentityAccountIdAsync (Guid identityAccountId) {
		return await _ctx.Users.SingleAsync(u => u.Id == identityAccountId);
	}

	public async Task<IdentityAccount?> ReadIdentityAccountByEmailAsync (string normalizedEmail) {
		return await _ctx.Users.SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
	}

}