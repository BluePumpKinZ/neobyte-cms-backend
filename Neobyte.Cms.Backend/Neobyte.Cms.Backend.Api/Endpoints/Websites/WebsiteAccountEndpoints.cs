using Neobyte.Cms.Backend.Core.Websites.Managers;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Domain.Websites;
using System.Linq;

namespace Neobyte.Cms.Backend.Api.Endpoints.Websites; 

internal class WebsiteUserEndpoints : IApiEndpoints {
	
	public string GroupName => "Website Users";
	public string Path => "/api/v1/websites/{websiteId:Guid}/users";
	public bool Authorized => true;
	
	public void RegisterApis (RouteGroupBuilder routes) {
		
		routes.MapGet("all", async (
				[FromServices] WebsiteAccountManager manager,
				[FromServices] Projector projector,
				[FromRoute] Guid websiteId) => {
					var websiteAccounts = await manager.GetAccountsByWebsiteIdAsync(new WebsiteId(websiteId));
					var projection = projector.Project<Account, AccountProjection>(websiteAccounts);
					return Results.Ok(projection);
				}).Authorize(UserPolicy.ClientPrivilege);
		
	}
}