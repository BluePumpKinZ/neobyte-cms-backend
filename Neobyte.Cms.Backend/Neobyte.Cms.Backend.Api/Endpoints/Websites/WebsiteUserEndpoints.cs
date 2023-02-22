using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Endpoints.Websites; 

internal class WebsiteUserEndpoints : IApiEndpoints {
	
	public string GroupName => "Website Users";
	public string Path => "/api/v1/websites/{websiteId:Guid}/users";
	public bool Authorized => true;
	
	public void RegisterApis (RouteGroupBuilder routes) {
		
		// routes.MapGet("all", async (
		// 		[FromServices] WebsiteAccountManager manager,
		// 		[FromServices] Projector projector,
		// 		[FromRoute] Guid websiteId) => {
		// 			var websiteAccounts = await manager.GetAllWebsiteUsersAsync(new WebsiteId(websiteId));
		// 			var projection = projector.Project<WebsiteAccount, WebsiteAccountProjection>(websiteAccounts);
		// 			return Results.Ok();
		// 		}).Authorize(UserPolicy.ClientPrivilege);
	}
}