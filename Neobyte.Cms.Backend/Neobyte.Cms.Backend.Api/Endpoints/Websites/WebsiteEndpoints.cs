using Neobyte.Cms.Backend.Core.Websites.Managers;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Endpoints.Websites;

internal class WebsiteEndpoints : IApiEndpoints {

	public string GroupName => "Websites";
	public string Path => "/api/v1/websites";

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapPost("create", async (
			[FromServices] WebsiteManager manager,
			[FromServices] Projector projector,
			[FromBody] WebsiteCreateRequestModel request) => {
				var website = await manager.AddWebsiteAsync(request);
				var projection = projector.Project<Website, WebsiteEditProjection>(website);
				return Results.Created(projection.Id.ToString(), projection);
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsiteCreateRequestModel>();

		routes.MapGet("{websiteId:Guid}", async (
			[FromServices] WebsiteManager manager,
			[FromServices] Projector projector,
			[FromRoute] Guid websiteId) => {
				var websites = await manager.GetWebsiteById(new WebsiteId(websiteId));
				var projection = projector.Project<Website, WebsiteEditProjection>(websites);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapGet("all", async (
			[FromServices] WebsiteManager manager,
			[FromServices] Projector projector) => {
				var websites = await manager.GetAllWebsitesAsync();
				var projection = projector.Project<Website, WebsiteProjection>(websites);
				return Results.Ok(projection);
		}).Authorize(UserPolicy.OwnerPrivilege);

		routes.MapPut("edit", async (
			[FromServices] WebsiteManager manager,
			[FromServices] Projector projector,
			[FromBody] WebsiteEditRequestModel request) => {
				var website = await manager.EditWebsiteAsync(request);
				var projection = projector.Project<Website, WebsiteEditProjection>(website);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.OwnerPrivilege)
		.ValidateBody<WebsiteEditRequestModel>();

	}

}