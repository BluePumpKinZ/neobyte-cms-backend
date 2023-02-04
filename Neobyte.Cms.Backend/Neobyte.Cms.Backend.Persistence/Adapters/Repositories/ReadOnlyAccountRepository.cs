using Microsoft.EntityFrameworkCore;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Persistence.EF;
using System.Linq;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

public class ReadOnlyAccountRepository : IReadOnlyAccountRepository {

	private readonly EFDbContext _ctx;

	public ReadOnlyAccountRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<Account> CreateAccountAsync (Account account) {
		return (await _ctx.Accounts.AddAsync(account)).Entity;
	}

	public async Task<IdentityAccount> ReadIdentityAccountWithAccountByEmail (string normalizedEmail) {
		return await _ctx.Users
			.Include(u => u.Account)
			.Where(u => u.NormalizedEmail == normalizedEmail.ToUpper())
			.SingleAsync();
	}

}