using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Api.Endpoints.Accounts;

public class AccountsListEndpoints : IApiEndpoints {

	public string GroupName => "Accounts List";
	public string Path => "/api/v1/accounts/list";

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
		}).Authorize(UserPolicy.OwnerPrivilege);

	}

}