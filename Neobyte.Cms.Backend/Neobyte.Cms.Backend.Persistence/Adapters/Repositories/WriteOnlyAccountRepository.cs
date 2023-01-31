using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Persistence.EF;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

public class WriteOnlyAccountRepository : IWriteOnlyAccountRepository {

	private readonly EFDbContext _ctx;

	public WriteOnlyAccountRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public Account UpdateAccount (Account account) {
		return _ctx.Accounts.Update(account).Entity;
	}

}