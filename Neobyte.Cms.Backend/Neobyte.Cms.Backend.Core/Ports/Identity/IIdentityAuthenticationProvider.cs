using Microsoft.AspNetCore.Http;
using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using Neobyte.Cms.Backend.Domain.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Identity;

public interface IIdentityAuthenticationProvider {

	public Task<(IdentityRegisterResponseModel.RegisterResult result, IEnumerable<string> errors)> RegisterAsync (IdentityRegisterRequestModel request);

	public Task<IdentityLoginResponseModel.LoginResult> LoginAsync (IdentityLoginRequestModel request);

	public string GenerateTokenForAccount (Account accountWithRoles, bool rememberMe);

	public Task<IdentityAuthenticateResponseModel> Authenticate (HttpContext httpContext);

}