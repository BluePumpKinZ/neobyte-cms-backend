using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Persistence.EF;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

public class WriteOnlyAccountRepository : IWriteOnlyAccountRepository {

	private readonly EFDbContext _ctx;

	public WriteOnlyAccountRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<Account> CreateAccountAsync (Account account) {
		return (await _ctx.Accounts.AddAsync(account)).Entity;
	}

	public Account UpdateAccount (Account account) {
		return _ctx.Accounts.Update(account).Entity;
	}

}