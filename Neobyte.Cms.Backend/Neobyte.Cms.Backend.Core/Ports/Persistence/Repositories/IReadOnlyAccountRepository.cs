using Neobyte.Cms.Backend.Domain.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IReadOnlyAccountRepository {

	public Task<Account?> ReadAccountByEmailAsync (string email);

	public Task<Account?> ReadAccountByEmailWithRolesAsync (string email);

}