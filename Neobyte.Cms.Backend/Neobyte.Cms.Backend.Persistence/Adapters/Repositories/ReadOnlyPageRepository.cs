﻿using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Persistence.EF;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories; 

internal class ReadOnlyPageRepository : IReadOnlyPageRepository {

	private readonly EFDbContext _ctx;

	public ReadOnlyPageRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<Page?> ReadPageByIdAsync (PageId pageId) {
		var entity = await _ctx.PageEntities.SingleOrDefaultAsync(x => x.Id == pageId);
		return entity?.ToDomain();
	}

	public async Task<Page?> ReadPageWithWebsiteByIdAsync (PageId pageId) {
		var entity = await _ctx.PageEntities
			.Include(x => x.Website)
			.SingleOrDefaultAsync(x => x.Id == pageId);
		var page = entity?.ToDomain();
		if (page is null)
			return null;

		page.Website = entity!.Website!.ToDomain();
		return page;
	}

	public async Task<IEnumerable<Page>> ReadPagesByWebsiteIdAsync (WebsiteId websiteId) {
		return await _ctx.PageEntities
			.Where(p => p.Website!.Id == websiteId)
			.Select(p => p.ToDomain()).ToListAsync();
	}



}