using Neobyte.Cms.Backend.Domain.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Identity;

public interface IIdentityAuthenticationProvider {

	public Task<bool> LoginAsync (string email, string password);

	public Task<string> GenerateJwtTokenAsync (IdentityAccount identityAccount, bool rememberMe);

	public string NormalizeEmail (string email);

}