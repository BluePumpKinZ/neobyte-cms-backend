using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Persistence.EF;
using Neobyte.Cms.Backend.Persistence.Entities.Websites;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

internal class WriteOnlyPageRepository : IWriteOnlyPageRepository {

	private readonly EFDbContext _ctx;

	public WriteOnlyPageRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<Page> CreatePageAsync (Page page) {

		var websiteEntity = await _ctx.WebsiteEntities.SingleAsync(w => page.Website!.Id == w.Id);
		var pageEntity = new PageEntity(page.Id, page.Name, page.Path, page.Created, page.Modified) { Website = websiteEntity };

		await _ctx.PageEntities.AddAsync(pageEntity);
		await _ctx.SaveChangesAsync();
		return page;
	}

	public async Task<Page> UpdatePageAsync (Page page) {
		var pageEntity = await _ctx.PageEntities.SingleAsync(p => p.Id == page.Id);
		pageEntity.Name = page.Name;
		pageEntity.Path = page.Path;
		pageEntity.Modified = page.Modified;
		await _ctx.SaveChangesAsync();
		return page;
	}

	public async Task DeletePageAsync (Page page) {
		var pageEntity = await _ctx.PageEntities.SingleAsync(p => p.Id == page.Id);
		_ctx.PageEntities.Remove(pageEntity);
		await _ctx.SaveChangesAsync();
	}

	

}