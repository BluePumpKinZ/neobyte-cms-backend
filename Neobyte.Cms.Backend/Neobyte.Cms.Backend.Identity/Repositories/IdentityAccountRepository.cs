using Microsoft.EntityFrameworkCore;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Persistence.EF;
using Neobyte.Cms.Backend.Persistence.Entities.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Identity.Repositories; 

public class IdentityAccountRepository {

	private readonly EFDbContext _ctx;

	public IdentityAccountRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<IdentityAccountEntity?> ReadIdentityAccountByEmailAsync (string normalizedEmail) {
		return await _ctx.Users
			.Include(u => u.Account)
			.SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
	}

	public async Task<IdentityAccountEntity> ReadIdentityAccountByAccountIdAsync (AccountId accountId) {
		return await _ctx.Users
			.Include(u => u.Account)
			.SingleAsync(u => u.Account!.Id == accountId);
	}

	public async Task<AccountEntity> ReadAccountByIdAsync (AccountId accountId) {
		return await _ctx.AccountEntities.SingleAsync(a => a.Id == accountId);
	}

}