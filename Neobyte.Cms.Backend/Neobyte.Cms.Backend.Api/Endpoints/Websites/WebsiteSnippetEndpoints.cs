using Neobyte.Cms.Backend.Core.Websites.Managers;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Endpoints.Websites;

internal class WebsiteSnippetEndpoints : IApiEndpoints {

	public string GroupName => "";
	public string Path => "/api/v1/websites/{websiteId:Guid}/snippets";

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapGet("", async (Guid websiteId,
			[FromServices] WebsiteSnippetManager manager,
			[FromServices] Projector projector) => {
				var snippets = await manager.GetWebsiteSnippetsAsync(new WebsiteId(websiteId));
				var projection = projector.Project<Snippet, SnippetProjection>(snippets);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.OwnerPrivilege);

	}

}