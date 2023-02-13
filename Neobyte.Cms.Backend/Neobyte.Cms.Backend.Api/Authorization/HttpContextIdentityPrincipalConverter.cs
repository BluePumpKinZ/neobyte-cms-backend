using Neobyte.Cms.Backend.Api.Exceptions;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Api.Authorization; 

public class HttpContextIdentityPrincipalConverter {

	private readonly IIdentityAuthorizationProvider _identityAuthorizationProvider;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public HttpContextIdentityPrincipalConverter (IIdentityAuthorizationProvider identityAuthorizationProvider, IHttpContextAccessor httpContextAccessor) {
		_identityAuthorizationProvider = identityAuthorizationProvider;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<Principal> GetPrincipalAsync () {
		var httpContext = _httpContextAccessor.HttpContext;
		if (httpContext is null)
			throw new AuthorizationException("HttpContext is could not be found");

		var headerGroup = httpContext.Request.Headers["Authorization"];
		if (headerGroup.Count != 1)
			throw new AuthorizationException("Authorization header is missing or invalid");

		var header = headerGroup[0]!;
		if (!header.StartsWith("Bearer "))
			throw new AuthorizationException("Bearer authorization header is missing or invalid");

		var token = header["Bearer ".Length..];
		var validationResult = await _identityAuthorizationProvider.ValidateTokenAsync(token);
		if (!validationResult.IsValid)
			throw new AuthorizationException("Authentication token is invalid, could not create principal");

		var accountId = new AccountId(Guid.Parse ((string)validationResult.Claims.First(c => c.Key == ClaimTypes.PrimarySid).Value));
		var roles = validationResult.Claims.Where(c => c.Key == ClaimTypes.Role).Select(c => (string)c.Value);

		return new Principal(accountId, roles);
	}

}