using Neobyte.Cms.Backend.Domain.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IWriteOnlyAccountRepository {

	public Account UpdateAccount (Account account);

	public Task<Account> CreateAccountAsync (Account account);

}