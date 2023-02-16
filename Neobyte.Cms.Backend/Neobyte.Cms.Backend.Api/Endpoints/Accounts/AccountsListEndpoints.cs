using Microsoft.Extensions.Logging;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Api.Endpoints.Accounts;

public class AccountsListEndpoints : IApiEndpoints {

	public string GroupName => "Accounts List";
	public string Path => "/api/v1/accounts/list";
	public bool Authorized => true;

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapGet("all", async ([FromServices] AccountListManager manager, [FromServices] Projector projector) => {
			var accounts = await manager.GetAllAccountsAsync();
			var projection = projector.Project<Account, AccountProjection>(accounts);
			return Results.Ok(projection);
		}).Authorize(UserPolicy.OwnerPrivilege);

		routes.MapPost("create", async ([FromServices] AccountManager manager, [FromBody] AccountsCreateRequestModel request) => {
			var result = await manager.CreateAccountAsync(request);
			if (!result.Success)
				return Results.BadRequest(result.Errors);

			return Results.Ok();
		}).Authorize(UserPolicy.OwnerPrivilege)
		.ValidateBody<AccountsCreateRequestModel>();

		routes.MapGet("{accountId:Guid}/details", async (
			[FromServices] AccountManager manager,
			[FromServices] Projector projector,
			[FromRoute] Guid accountId) => {
				var account = await manager.GetAccountDetails(new AccountId(accountId));
				var projection = projector.Project<Account, AccountProjection>(account);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.OwnerPrivilege);

		routes.MapPut("{accountId:Guid}/edit", async (
			[FromServices] AccountListManager manager,
			[FromServices] Projector projector,
			[FromBody] AccountChangeDetailsOwnerRequestModel request,
			[FromServices] Principal principal,
			[FromRoute] Guid accountId) => {
				request.AccountId = new AccountId(accountId);
				if (principal.AccountId == new AccountId())
					return Results.BadRequest("You cannot edit your own account.");
				
				var account = await manager.EditAccountDetailsAsync(request);
				var projection = projector.Project<Account, AccountProjection>(account);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.OwnerPrivilege);

		routes.MapDelete("{accountId:Guid}/delete", async (
			[FromServices] AccountListManager manager,
			[FromServices] Principal principal,
			[FromServices] ILogger<AccountsListEndpoints> logger,
			[FromRoute] Guid accountId) => {
				if (principal.AccountId == new AccountId(accountId))
					return Results.BadRequest("You cannot delete your own account.");

				await manager.DeleteAccountAsync(new AccountId(accountId));
				return Results.Ok(new { Message = "Account deleted" });
			}).Authorize(UserPolicy.OwnerPrivilege);

	}

}