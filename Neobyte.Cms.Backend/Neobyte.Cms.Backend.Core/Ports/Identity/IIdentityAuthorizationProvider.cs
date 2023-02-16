using Microsoft.IdentityModel.Tokens;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Core.Ports.Identity;

public interface IIdentityAuthorizationProvider {

	public Task<TokenValidationResult> ValidateTokenAsync (string token);

	public Task<bool> CanLoginAsync (AccountId accountId);

}