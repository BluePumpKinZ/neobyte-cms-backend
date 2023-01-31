using Neobyte.Cms.Backend.Domain.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IWriteOnlyAccountRepository {

	public Task<Account> CreateAccountAsync (Account account);

	public Account UpdateAccount (Account account);

}