using Neobyte.Cms.Backend.Domain.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IReadOnlyAccountRepository {

	public Task<Account> CreateAccountAsync (Account account);

	public Task<IdentityAccount> ReadIdentityAccountWithAccountByEmail (string normalizedEmail);

}