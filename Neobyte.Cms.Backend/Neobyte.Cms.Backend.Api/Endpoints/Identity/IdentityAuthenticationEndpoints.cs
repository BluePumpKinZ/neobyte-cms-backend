using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Neobyte.Cms.Backend.Core.Identity.Managers;
using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;

namespace Neobyte.Cms.Backend.Api.Endpoints.Identity;


public class IdentityAuthenticationEndpoints : IApiEndpoints {

	public string Path => "/api/v1/identity/authentication";

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapPost("register", async ([FromServices] IdentityAuthenticationManager manager,
			[FromBody] IdentityRegisterRequestModel request) => {
				var response = await manager.Register(request);
				return response.Result switch {
					IdentityRegisterResponseModel.RegisterResult.Success => Results.Ok("Account created"),
					IdentityRegisterResponseModel.RegisterResult.Failed => Results.BadRequest(response.Errors),
					IdentityRegisterResponseModel.RegisterResult.RequiresConfirmation => Results.Redirect("register/confirm"),
					_ => throw new UnreachableException()
				};
			});

		routes.MapPost("login", async ([FromServices] IdentityAuthenticationManager manager,
			[FromBody] IdentityLoginRequestModel request) => {
				var response = await manager.Login(request);
				return response.Result switch {
					IdentityLoginResponseModel.LoginResult.Success => Results.Ok(),
					IdentityLoginResponseModel.LoginResult.RequiresTwoFactor => Results.Redirect("login/2fa"),
					IdentityLoginResponseModel.LoginResult.LockedOut => Results.BadRequest("Locked out"),
					IdentityLoginResponseModel.LoginResult.BadCredentials => Results.BadRequest("Bad credentials"),
					IdentityLoginResponseModel.LoginResult.NotAllowed => Results.Unauthorized(),
					IdentityLoginResponseModel.LoginResult.Unknown => Results.BadRequest(),
					_ => throw new UnreachableException()
				};
			});

		routes.MapDelete("logout", async ([FromServices] IdentityAuthenticationManager manager) => {
			await manager.Logout();
		});
	}

}