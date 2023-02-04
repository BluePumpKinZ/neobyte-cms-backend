using Microsoft.AspNetCore.Http;
using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using Neobyte.Cms.Backend.Domain.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Identity;

public interface IIdentityAuthenticationProvider {

	public Task<IdentityLoginResponseModel.LoginResult> LoginAsync (IdentityLoginRequestModel request);

	public string GenerateTokenForAccount (Account accountWithRoles, bool rememberMe);

	public Task<IdentityAuthenticateResponseModel> AuthenticateAsync (HttpContext httpContext);

	public Task<(bool valid, string[]? errors)> UpdateAccountPasswordAsync (Account account, string newPassword);

}