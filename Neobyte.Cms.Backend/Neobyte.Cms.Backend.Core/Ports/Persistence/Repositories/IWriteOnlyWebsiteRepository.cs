using Neobyte.Cms.Backend.Domain.Websites;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IWriteOnlyWebsiteRepository {

	public Task<Website> CreateWebsiteAsync (Website website);

}