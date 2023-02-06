using Neobyte.Cms.Backend.Api.Authorization;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
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

	}

}