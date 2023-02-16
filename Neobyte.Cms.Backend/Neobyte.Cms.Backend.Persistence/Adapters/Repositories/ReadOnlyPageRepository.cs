using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Persistence.EF;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

internal class ReadOnlyPageRepository : IReadOnlyPageRepository {

	private readonly EFDbContext _ctx;

	public ReadOnlyPageRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<Page?> GetPageByIdAsync (PageId pageId) {
		var entity = await _ctx.PageEntities.SingleOrDefaultAsync(x => x.Id == pageId);
		if (entity is null)
			return null;

		return new Page(entity.Id, entity.Name, entity.Path, entity.Created, entity.Modified);
	}

	public async Task<IEnumerable<Page>> GetPagesByWebsiteIdAsync (WebsiteId websiteId) {
		return await _ctx.PageEntities
			.Where(p => p.Website!.Id == websiteId)
			.Select(p => new Page(p.Id, p.Name, p.Path, p.Created, p.Modified)).ToListAsync();
	}



}