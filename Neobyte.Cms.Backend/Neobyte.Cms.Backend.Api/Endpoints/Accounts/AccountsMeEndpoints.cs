﻿using Neobyte.Cms.Backend.Api.Authorization;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Api.Endpoints.Accounts;

public class AccountsMeEndpoints : IApiEndpoints {

	public string GroupName => "Account";
	public string Path => "/api/v1/accounts/me";

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapGet("details", async (
			[FromServices] AccountManager manager,
			[FromServices] Projector projector,
			[FromServices] Principal principal) => {
				var account = await manager.GetAccountDetails(principal.AccountId);
				var projection = projector.Project<Account, AccountProjection>(account);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapPut("change-password", async (
			[FromServices] AccountManager manager,
			[FromServices] Principal principal,
			[FromBody] AccountChangePasswordRequestModel request) => {
				var response = await manager.ChangePasswordAsync(request, principal.IdentityAccountId);
				return response.Success
					? Results.Ok(new { Message = "Password updated" })
					: Results.BadRequest(new { response.Errors });
			}).Authorize(UserPolicy.ClientPrivilege)
			.ValidateBody<AccountChangePasswordRequestModel>();

		routes.MapPut("change-details", async (
			[FromServices] AccountManager manager,
			[FromServices] Principal principal,
			[FromBody] AccountChangeDetailsRequestModel request) => {
				await manager.ChangeDetailsAsync(request, principal.AccountId);
				return Results.Ok(new { Message = "Details updated" });
			}).Authorize(UserPolicy.ClientPrivilege)
			.ValidateBody<AccountChangeDetailsRequestModel>();

		routes.MapPut("change-email", async (
			[FromServices] AccountManager manager,
			[FromServices] Principal principal,
			[FromQuery] string email) => {
				bool success = await manager.ChangeEmailAsync(email, principal.IdentityAccountId);
				return success ? Results.Ok(new { Message = "Email updated" }) : Results.BadRequest(new { Message = "Email already in use" });
			}).Authorize(UserPolicy.ClientPrivilege);
	}

}