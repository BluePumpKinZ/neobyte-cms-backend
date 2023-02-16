using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IWriteOnlyPageRepository {

	public Task<Page> AddPageAsync (Page page);

	public Task DeletePageAsync (Page page);

}