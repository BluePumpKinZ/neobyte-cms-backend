using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Persistence.EF;
using Neobyte.Cms.Backend.Persistence.Entities.Websites;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories;

internal class WriteOnlySnippetRepository : IWriteOnlySnippetRepository {

	private readonly EFDbContext _ctx;

	public WriteOnlySnippetRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<Snippet> CreateSnippetAsync (Snippet snippet) {
		var websiteEntity = await _ctx.WebsiteEntities.SingleAsync(x => x.Id == snippet.Website!.Id);
		var contentEntity = new HtmlContentEntity(snippet.Content!.Id, snippet.Content.Html);
		var snippetEntity = new SnippetEntity(snippet.Id, snippet.Name, snippet.Description) {
			Website = websiteEntity, Content = contentEntity
		};
		await _ctx.SnippetEntities.AddAsync(snippetEntity);
		await _ctx.SaveChangesAsync();
		return snippet;
	}

	public async Task<Snippet> UpdateSnippetAsync (Snippet snippet) {
		var snippetEntity = await _ctx.SnippetEntities.SingleAsync(x => x.Id == snippet.Id);
		snippetEntity.Apply(snippet);
		_ctx.SnippetEntities.Update(snippetEntity);
		await _ctx.SaveChangesAsync();
		return snippet;
	}

	public async Task DeleteSnippetAsync (Snippet snippet) {
		var snippetEntity = await _ctx.SnippetEntities.SingleAsync(x => x.Id == snippet.Id);
		_ctx.SnippetEntities.Remove(snippetEntity);
		await _ctx.SaveChangesAsync();
	}

}