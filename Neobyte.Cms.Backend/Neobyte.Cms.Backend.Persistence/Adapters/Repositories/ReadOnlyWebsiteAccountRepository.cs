using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Persistence.EF;
using Neobyte.Cms.Backend.Persistence.Entities.Websites;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

public class ReadOnlyWebsiteAccountRepository : IReadOnlyWebsiteAccountRepository {
	
	private readonly EFDbContext _ctx;
	
	public ReadOnlyWebsiteAccountRepository (EFDbContext ctx) {
		_ctx = ctx;
	}
	
	public async Task<IEnumerable<Account>> ReadAccountsByWebsiteIdAsync (WebsiteId websiteId) {
		var userRoles = await (from u in _ctx.Users
			join ur in _ctx.UserRoles on u.Id equals ur.UserId
			join r in _ctx.Roles on ur.RoleId equals r.Id
			join wa in _ctx.WebsiteAccountEntities on u.Account!.Id equals wa.Account!.Id
			where wa.Website!.Id == websiteId
			group r.Name by u into g
			select new { g.Key.Id, Roles = g.ToArray()})
			.ToListAsync();

		var accounts = await (from u in _ctx.Users
			join a in _ctx.AccountEntities on u.Account!.Id equals a.Id
			join wa in _ctx.WebsiteAccountEntities on a.Id equals wa.Account!.Id
			where wa.Website!.Id == websiteId
			select new { Account = a, u.Email, IdentityAccountEntityId = u.Id }).ToListAsync();

		return accounts
			.Select(a => {
			string[] roles = (userRoles
				.SingleOrDefault(ur => ur.Id == a.IdentityAccountEntityId)?.Roles ?? Array.Empty<string>())!;
			return new Account(a.Account.Id, a.Email!, a.Account.Username, a.Account.Bio, a.Account.Enabled, a.Account.CreationDate, roles);
		});
	}

	public async Task<IEnumerable<Website>> ReadWebsitesByAccountIdAsync (AccountId accountId) {
		return await _ctx.WebsiteAccountEntities
			.Where(w => w.Account!.Id == accountId)
			.Select(w => w.Website!.ToDomain())
			.ToListAsync();
	}

	public async Task<WebsiteAccount?> ReadWebsiteAccountByWebsiteIdAndAccountIdAsync (WebsiteId websiteId, AccountId accountId) {
		return await _ctx.WebsiteAccountEntities
			.Where(w => w.Website!.Id == websiteId && w.Account!.Id == accountId)
			.Select(w => w.ToDomain())
			.SingleOrDefaultAsync();
	}
}