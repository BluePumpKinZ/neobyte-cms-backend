using Microsoft.EntityFrameworkCore;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Persistence.EF;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

public class ReadOnlyAccountRepository : IReadOnlyAccountRepository {

	private readonly EFDbContext _ctx;

	public ReadOnlyAccountRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<Account?> ReadAccountByEmailAsync (string email) {
		return await _ctx.Accounts.SingleOrDefaultAsync(a => a.Email == email);
	}

	public async Task<Account?> ReadAccountByEmailWithRolesAsync (string email) {
		return await _ctx.Accounts.Include(a => a.AccountRoles!)
			.ThenInclude(ar => ar.Role).SingleOrDefaultAsync(a => a.Email == email);
	}

	public async Task<Account> CreateAccountAsync (Account account) {
		return (await _ctx.Accounts.AddAsync(account)).Entity;
	}

}