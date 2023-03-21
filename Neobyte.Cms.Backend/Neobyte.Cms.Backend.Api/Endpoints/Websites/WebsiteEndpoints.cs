using Neobyte.Cms.Backend.Core.Websites.Managers;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Websites;
using System.Collections.Generic;
using System.Linq;

namespace Neobyte.Cms.Backend.Api.Endpoints.Websites;

internal class WebsiteEndpoints : IApiEndpoints {
	public string GroupName => "Websites";
	public string Path => "/api/v1/websites";
	public bool Authorized => true;

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
				var website = await manager.GetWebsiteByIdAsync(new WebsiteId(websiteId));
				var projection = projector.Project<Website, WebsiteEditProjection>(website);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapGet("all", async (
			[FromServices] WebsiteManager manager,
			[FromServices] WebsiteAccountManager websiteAccountManager,
			[FromServices] Principal principal,
			[FromServices] Projector projector) => {
				bool isOwner = principal.Roles.Any(r => r == Role.Owner.RoleName);
				IEnumerable<Website> websites;
				if (isOwner) {
					websites = await manager.GetAllWebsitesAsync();
				} else {
					websites = await websiteAccountManager.GetWebsitesByAccountIdAsync(principal.AccountId);
				}
				var projection = projector.Project<Website, WebsiteProjection>(websites);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapPut("edit", async (
				[FromServices] WebsiteManager manager,
				[FromServices] Projector projector,
				[FromBody] WebsiteEditRequestModel request) => {
					var website = await manager.EditWebsiteAsync(request);
					var projection = projector.Project<Website, WebsiteEditProjection>(website);
					return Results.Ok(projection);
				}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsiteEditRequestModel>();

		routes.MapDelete("{websiteId:Guid}/delete", async (
			[FromServices] WebsiteManager manager,
			[FromRoute] Guid websiteId) => {
				await manager.DeleteWebsiteAsync(new WebsiteId(websiteId));
				return Results.Ok();
			}).Authorize(UserPolicy.OwnerPrivilege);
	}
}