using Microsoft.IdentityModel.Tokens;

namespace Neobyte.Cms.Backend.Core.Ports.Identity;

public interface IIdentityAuthorizationProvider {

	public Task<TokenValidationResult> ValidateTokenAsync (string token);

}