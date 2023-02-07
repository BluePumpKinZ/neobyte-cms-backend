using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Identity.Managers;
using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;

namespace Neobyte.Cms.Backend.Api.Endpoints.Identity;

public class IdentityAuthenticationEndpoints : IApiEndpoints {

	public string GroupName => "Authentication";
	public string Path => "/api/v1/identity/authentication";

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapPost("login", async ([FromBody] IdentityLoginRequestModel request,
			[FromServices] IdentityAuthenticationManager manager) => {
				var response = await manager.LoginAsync(request);
				if (response.Authenticated)
					return Results.Ok(new { response.Token, response.Expires });

				return Results.Unauthorized();
			}).ValidateBody<IdentityLoginRequestModel>()
			.AllowAnonymous();

		routes.MapGet("check-login", () => Results.Ok())
			.Authorize(UserPolicy.ClientPrivilege);

	}

}