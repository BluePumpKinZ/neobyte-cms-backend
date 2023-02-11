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
		return await (from u in _ctx.Users
					  join a in _ctx.AccountEntities on u.Account!.Id equals a.Id
					  join ur in _ctx.UserRoles on u.Id equals ur.UserId into urGroup
					  from ur in urGroup.DefaultIfEmpty()
					  join r in _ctx.Roles on ur.RoleId equals r.Id into rGroup
					  from r in rGroup.DefaultIfEmpty()
					  where u.NormalizedEmail == normalizedEmail
					  select new { Account = a, u.Email, Role = r } into result
					  group result by result.Account into g
					  select new Account(g.Key.Id, g.First().Email, g.Key.Username, g.Key.Bio, g.Key.CreationDate, g.Where(r => r.Role != null).Select(r => r.Role.Name!).ToArray())).SingleOrDefaultAsync();
	}


	public async Task<IEnumerable<Account>> ReadAllAccountsAsync () {
		return await (from u in _ctx.Users
					  join a in _ctx.AccountEntities on u.Account!.Id equals a.Id
					  join ur in _ctx.UserRoles on u.Id equals ur.UserId into urGroup
					  from ur in urGroup.DefaultIfEmpty()
					  join r in _ctx.Roles on ur.RoleId equals r.Id into rGroup
					  from r in rGroup.DefaultIfEmpty()
					  select new { Account = a, u.Email, Role = r } into result
					  group result by result.Account into g
					  select new Account(g.Key.Id, g.First().Email, g.Key.Username, g.Key.Bio, g.Key.CreationDate, g.Where(r => r.Role != null).Select(r => r.Role.Name!).ToArray()))
					  .ToListAsync();
	}

	public async Task<Account> ReadAccountById (AccountId accountId) {
		return await(from u in _ctx.Users
					 join a in _ctx.AccountEntities on u.Account!.Id equals a.Id
					 join ur in _ctx.UserRoles on u.Id equals ur.UserId into urGroup
					 from ur in urGroup.DefaultIfEmpty()
					 join r in _ctx.Roles on ur.RoleId equals r.Id into rGroup
					 from r in rGroup.DefaultIfEmpty()
					 where a.Id == accountId
					 select new { Account = a, u.Email, Role = r } into result
					 group result by result.Account into g
					 select new Account(g.Key.Id, g.First().Email, g.Key.Username, g.Key.Bio, g.Key.CreationDate, g.Where(r => r.Role != null).Select(r => r.Role.Name!).ToArray())).SingleAsync();
	}


}