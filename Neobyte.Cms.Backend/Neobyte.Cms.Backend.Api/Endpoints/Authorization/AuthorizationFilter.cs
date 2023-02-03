using Microsoft.AspNetCore.Http;
using Neobyte.Cms.Backend.Core.Identity.Managers;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Api.Endpoints.Authorization; 

internal class AuthorizationFilter : IEndpointFilter {

	public const string MetadataKey = "authorization-metadata";

	private readonly IdentityAuthenticationManager _authenticationManager;
	private readonly IdentityAuthorizationManager _authorizationManager;

	public AuthorizationFilter (IdentityAuthenticationManager authenticationManager, IdentityAuthorizationManager authManager) {
		_authenticationManager = authenticationManager;
		_authorizationManager = authManager;
	}

	public async ValueTask<object?> InvokeAsync (EndpointFilterInvocationContext context, EndpointFilterDelegate next) {

		var nullableAuthorizationMetadata = (AuthorizationMetadata?)context.HttpContext.Items[MetadataKey];

		if (nullableAuthorizationMetadata is null)
			throw new TaskCanceledException("Canceled request at {} because authorization metadata was null");

		var authorizationMetadata = nullableAuthorizationMetadata.Value;
		var authenticationResult = await _authenticationManager.AuthenticateAsync(context.HttpContext);

		if (!authenticationResult.IsAuthenticated)
			return Results.Unauthorized();

		bool authorized = _authorizationManager.IsAuthorized(authorizationMetadata.PolicyName, authenticationResult.Roles!);
		if (!authorized)
			return Results.Unauthorized();

		return await next(context);
	}

}