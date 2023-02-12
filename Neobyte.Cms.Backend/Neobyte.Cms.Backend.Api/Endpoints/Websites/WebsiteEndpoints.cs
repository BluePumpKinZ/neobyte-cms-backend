using Neobyte.Cms.Backend.Core.Websites.Managers;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Endpoints.Websites;

public class WebsiteEndpoints : IApiEndpoints {

	public string GroupName => "Websites";
	public string Path => "/api/v1/websites";

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapGet("{id:Guid}", async (
			[FromServices] WebsiteManager manager,
			[FromServices] Projector projector,
			[FromRoute] Guid id) => {
				var websiteId = new WebsiteId(id);
				var websites = await manager.GetWebsiteById(websiteId);
				var projection = projector.Project<Website, WebsiteProjection>(websites);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapGet("all", async (
			[FromServices] WebsiteManager manager,
			[FromServices] Projector projector) => {

				var websites = await manager.GetAllWebsitesAsync();
				var projection = projector.Project<Website, WebsiteProjection>(websites);
				return Results.Ok(projection);

			}).Authorize(UserPolicy.OwnerPrivilege);

	}

}