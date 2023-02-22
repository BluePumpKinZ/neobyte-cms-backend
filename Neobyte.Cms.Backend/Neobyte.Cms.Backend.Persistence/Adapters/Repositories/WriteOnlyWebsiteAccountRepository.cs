using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Persistence.EF;
using Neobyte.Cms.Backend.Persistence.Entities.Websites;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories;

public class WriteOnlyWebsiteAccountRepository : IWriteOnlyWebsiteAccountRepository {
	
	private readonly EFDbContext _ctx;
	
	public WriteOnlyWebsiteAccountRepository (EFDbContext ctx) {
		_ctx = ctx;
	}
	
	public async Task<WebsiteAccount> CreateWebsiteAccountAsync (WebsiteAccount websiteAccount) {
		var websiteEntity = await _ctx.WebsiteEntities.SingleAsync(x => x.Id == websiteAccount.Website!.Id);
		var accountEntity = await _ctx.AccountEntities.SingleAsync(x => x.Id == websiteAccount.Account!.Id);
		var websiteAccountEntity = new WebsiteAccountEntity(websiteAccount.Id, websiteAccount.CreatedDate) {
			Website = websiteEntity,
			Account = accountEntity
		};
		await _ctx.WebsiteAccountEntities.AddAsync(websiteAccountEntity);
		await _ctx.SaveChangesAsync();
		return websiteAccount;
	}

	public async Task DeleteWebsiteAccountAsync (WebsiteAccount websiteAccount) {
		var websiteAccountEntity = await _ctx.WebsiteAccountEntities.SingleAsync(x => x.Id == websiteAccount.Id);
		_ctx.WebsiteAccountEntities.Remove(websiteAccountEntity);
		await _ctx.SaveChangesAsync();
	}
}