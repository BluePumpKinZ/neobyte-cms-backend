using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IWriteOnlySnippetRepository {

	public Task<Snippet> CreateSnippetAsync (Snippet snippet);

	public Task<Snippet> UpdateSnippetAsync (Snippet snippet);

	public Task DeleteSnippetAsync (Snippet snippet);

}