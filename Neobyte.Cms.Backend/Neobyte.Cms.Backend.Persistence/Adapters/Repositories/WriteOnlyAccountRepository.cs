using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Persistence.EF;
using Neobyte.Cms.Backend.Persistence.Entities.Accounts;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

public class WriteOnlyAccountRepository : IWriteOnlyAccountRepository {

	private readonly EFDbContext _ctx;

	public WriteOnlyAccountRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<Account> UpdateAccountAsync (Account account) {
		var accountEntity = await _ctx.AccountEntities.SingleAsync(a => a.Id == account.Id);
		accountEntity.Username = account.Username;
		accountEntity.Bio = account.Bio;

		IdentityAccountEntity identityAccountEntity = await (from u in _ctx.Users
										   join a in _ctx.AccountEntities on u.Account!.Id equals a.Id
										   select u).SingleAsync();

		identityAccountEntity.Email = account.Email;
		identityAccountEntity.NormalizedEmail = account.Email.ToUpper();

		await _ctx.SaveChangesAsync();
		return account;
	}

	public async Task DeleteAccountByIdAsync (AccountId accountId) {

		var identityAccountEntity = _ctx.Users.SingleOrDefault(u => u.Account!.Id == accountId);
		var accountEntity = _ctx.AccountEntities.SingleOrDefault(a => a.Id == accountId);
		if (identityAccountEntity is null || accountEntity is null)
			throw new AccountNotFoundException($"Account {accountId} could not be found");

		_ctx.Users.Remove(identityAccountEntity);
		_ctx.AccountEntities.Remove(accountEntity);

		await _ctx.SaveChangesAsync();
	}

}