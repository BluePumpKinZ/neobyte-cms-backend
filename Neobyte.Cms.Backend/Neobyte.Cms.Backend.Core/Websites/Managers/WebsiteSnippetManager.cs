using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsiteSnippetManager {

	private readonly IReadOnlySnippetRepository _readOnlySnippetRepository;
	private readonly IWriteOnlySnippetRepository _writeOnlySnippetRepository;
	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;

	public WebsiteSnippetManager (IReadOnlySnippetRepository readOnlySnippetRepository, IWriteOnlySnippetRepository writeOnlySnippetRepository, IReadOnlyWebsiteRepository readOnlyWebsiteRepository) {
		_readOnlySnippetRepository = readOnlySnippetRepository;
		_writeOnlySnippetRepository = writeOnlySnippetRepository;
		_readOnlyWebsiteRepository = readOnlyWebsiteRepository;
	}

	public async Task<IEnumerable<Snippet>> GetWebsiteSnippetsAsync (WebsiteId websiteId) {
		return await _readOnlySnippetRepository.ReadAllSnippetsByWebsiteId(websiteId);
	}

	public async Task<Snippet> AddWebsiteSnippetAsync (WebsiteCreateSnippetRequestModel request) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(request.WebsiteId)
			?? throw new WebsiteNotFoundException($"Website {request.WebsiteId} not found");
		var content = new HtmlContent(request.Content);
		var snippet = new Snippet(request.Name, request.Description) { Website = website, Content = content };
		return await _writeOnlySnippetRepository.CreateSnippetAsync(snippet);
	}

	public async Task<Snippet> GetWebsiteSnippetAsync (WebsiteId websiteId, SnippetId snippetId) {
		var snippet = await _readOnlySnippetRepository.ReadSnippetWithWebsiteByIdAsync(snippetId)
			?? throw new SnippetNotFoundException($"Snippet {snippetId} not found");

		if (snippet.Website!.Id != websiteId)
			throw new SnippetNotFoundException($"Snippet {snippetId} not found");

		return snippet;
	}

	public async Task DeleteSnippetAsync (WebsiteId websiteId, SnippetId snippetId) {
		var snippet = await _readOnlySnippetRepository.ReadSnippetWithWebsiteByIdAsync(snippetId)
			?? throw new SnippetNotFoundException($"Snippet {snippetId} not found");

		if (snippet.Website!.Id != websiteId)
			throw new SnippetNotFoundException($"Snippet {snippetId} not found");

		await _writeOnlySnippetRepository.DeleteSnippetAsync(snippet);
	}

	public async Task<Snippet> EditSnippetAsync (WebsiteSnippetEditRequestModel request) {
		var snippet = await _readOnlySnippetRepository.ReadSnippetWithWebsiteByIdAsync(request.SnippetId)
			?? throw new SnippetNotFoundException($"Snippet {request.SnippetId} not found");

		if (snippet.Website!.Id != request.WebsiteId)
			throw new SnippetNotFoundException($"Snippet {request.SnippetId} not found");

		snippet.Name = request.Name;
		snippet.Description = request.Description;
		snippet.Content!.Html = request.Content;

		return await _writeOnlySnippetRepository.UpdateSnippetAsync(snippet);
	}

}