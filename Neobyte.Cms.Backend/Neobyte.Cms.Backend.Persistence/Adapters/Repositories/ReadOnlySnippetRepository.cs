using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Persistence.EF;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories;

internal class ReadOnlySnippetRepository : IReadOnlySnippetRepository {

	private readonly EFDbContext _ctx;

	public ReadOnlySnippetRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<IEnumerable<Snippet>> ReadAllSnippetsByWebsiteId (WebsiteId websiteId) {
		return await _ctx.SnippetEntities
			.Where(s => s.Website != null && s.Website.Id == websiteId)
			.Select(s => new Snippet(s.Id, s.Name, s.Description))
			.ToListAsync();
	}

}