using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
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
				Account account;
				try {
					account = await manager.GetAccountDetails(principal.AccountId);
				} catch (NotFoundException e) {
					return Results.NotFound(new { e.Message });
				} catch (ApplicationException e) {
					return Results.BadRequest(new { e.Message });
				}
				var projection = projector.Project<Account, AccountProjection>(account);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapPut("change-details", async (
			[FromServices] AccountManager manager,
			[FromServices] Principal principal,
			[FromBody] AccountChangeDetailsRequestModel request) => {
				try {
					await manager.ChangeDetailsAsync(request, principal.AccountId);
				} catch (NotFoundException e) {
					return Results.NotFound(new { e.Message });
				} catch (ApplicationException e) {
					return Results.BadRequest(new { e.Message });
				}
				return Results.Ok(new { Message = "Details updated" });
			}).Authorize(UserPolicy.ClientPrivilege)
			.ValidateBody<AccountChangeDetailsRequestModel>();

		routes.MapPut("change-password", async (
			[FromServices] AccountManager manager,
			[FromServices] Principal principal,
			[FromBody] AccountChangePasswordRequestModel request) => {
				var response = await manager.ChangePasswordAsync(request, principal.AccountId);
				return response.Success
					? Results.Ok(new { Message = "Password updated" })
					: Results.BadRequest(new { response.Errors });
			}).Authorize(UserPolicy.ClientPrivilege)
			.ValidateBody<AccountChangePasswordRequestModel>();

	}

}