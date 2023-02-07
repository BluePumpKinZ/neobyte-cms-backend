using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Identity;

public interface IIdentityAuthorizationProvider {

	public Task<TokenValidationResult> ValidateTokenAsync (string token);

}