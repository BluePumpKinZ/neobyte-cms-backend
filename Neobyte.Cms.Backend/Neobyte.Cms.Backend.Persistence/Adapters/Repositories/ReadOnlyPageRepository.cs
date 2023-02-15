using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Persistence.EF;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

public class ReadOnlyPageRepository : IReadOnlyPageRepository {

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

}