using Microsoft.AspNetCore.Http;
using Neobyte.Cms.Backend.Api.Endpoints.Validation.Extensions;
using Neobyte.Cms.Backend.Core.Identity.Managers;
using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using System.Diagnostics;

namespace Neobyte.Cms.Backend.Api.Endpoints.Identity;

public class IdentityAuthenticationEndpoints : IApiEndpoints {

	public string GroupName => "Authentication";
	public string Path => "/api/v1/identity/authentication";

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapPost("login", async ([FromServices] IdentityAuthenticationManager manager,
			[FromBody] IdentityLoginRequestModel request) => {
				var response = await manager.Login(request);
				return response.Result switch {
					IdentityLoginResponseModel.LoginResult.Success => Results.Ok("Succesfull login"),
					IdentityLoginResponseModel.LoginResult.InvalidCredentials => Results.BadRequest("Bad credentials"),
					IdentityLoginResponseModel.LoginResult.NotAllowed => Results.BadRequest("Not allowed"),
					IdentityLoginResponseModel.LoginResult.Unknown => Results.BadRequest("Something went wrong"),
					_ => throw new UnreachableException()
				};
			}).ValidateBody<IdentityLoginRequestModel>();
	}

}