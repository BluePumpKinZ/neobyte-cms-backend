using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Persistence.EF;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories;

public class ReadOnlyAccountRepository : IReadOnlyAccountRepository {

	private readonly EFDbContext _ctx;

	public ReadOnlyAccountRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<bool> ReadOwnerAccountExistsAsync () {
		Guid ownerRoleId = (await _ctx.Roles.SingleAsync(r => r.Name == "Owner")).Id;
		return await _ctx.Users
			.Where(u => u.Account != null)
			.AnyAsync(u => _ctx.UserRoles
				.Where(ur => ur.UserId == u.Id)
				.Any(ur => ur.RoleId == ownerRoleId));
	}

	public async Task<Account?> ReadAccountByEmailAsync (string normalizedEmail) {
		var roles = await (from u in _ctx.Users
						   join ur in _ctx.UserRoles on u.Id equals ur.UserId
						   join r in _ctx.Roles on ur.RoleId equals r.Id
						   where u.NormalizedEmail == normalizedEmail
						   select r.Name).ToArrayAsync();

		return await _ctx.Users
			.Include(u => u.Account)
			.Where(u => u.NormalizedEmail == normalizedEmail)
			.Select(u => new Account(u.Account!.Id, u.Email!, u.Account!.Username, u.Account.Bio, u.Account.CreationDate, roles!)).SingleOrDefaultAsync();
	}


	public async Task<IEnumerable<Account>> ReadAllAccountsAsync () {

		var userRoles = await (from u in _ctx.Users
							   join ur in _ctx.UserRoles on u.Id equals ur.UserId
							   join r in _ctx.Roles on ur.RoleId equals r.Id
							   group r.Name by u into g
							   select new { g.Key.Id, Roles = g.ToArray() }).ToListAsync();

		var accounts = await (from u in _ctx.Users
							  join a in _ctx.AccountEntities on u.Account!.Id equals a.Id
							  select new { Account = a, u.Email, IdentityAccountEntityId = u.Id }).ToListAsync();

		return accounts.Select(a => {
			string[] roles = (userRoles.SingleOrDefault(ur => ur.Id == a.IdentityAccountEntityId)?.Roles ?? new string[0])!;
			return new Account(a.Account.Id, a.Email!, a.Account.Username, a.Account.Bio, a.Account.CreationDate, roles);
		});
	}

	public async Task<Account?> ReadAccountByIdAsync (AccountId accountId) {

		var roles = await (from u in _ctx.Users
						   join ur in _ctx.UserRoles on u.Id equals ur.UserId
						   join r in _ctx.Roles on ur.RoleId equals r.Id
						   where u.Account!.Id == accountId
						   select r.Name).ToArrayAsync();

		return await _ctx.Users
			.Include(u => u.Account)
			.Where(u => u.Account!.Id == accountId)
			.Select(u => new Account(u.Account!.Id, u.Email!, u.Account!.Username, u.Account.Bio, u.Account.CreationDate, roles!))
			.SingleOrDefaultAsync();
	}
}