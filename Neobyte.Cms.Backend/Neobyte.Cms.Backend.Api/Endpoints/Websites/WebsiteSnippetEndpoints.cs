using Neobyte.Cms.Backend.Core.Websites.Managers;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Endpoints.Websites;

internal class WebsiteSnippetEndpoints : IApiEndpoints {

	public string GroupName => "";
	public string Path => "/api/v1/websites/{websiteId:Guid}/snippets";
	public bool Authorized => true;

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapPost("add", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsiteSnippetManager manager,
			[FromServices] Projector projector,
			[FromBody] WebsiteCreateSnippetRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				var snippet = await manager.AddWebsiteSnippetAsync(request);
				var projection = projector.Project<Snippet, SnippetEditProjection>(snippet);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsiteCreateSnippetRequestModel>();

		routes.MapGet("", async (Guid websiteId,
			[FromServices] WebsiteSnippetManager manager,
			[FromServices] Projector projector) => {
				var snippets = await manager.GetWebsiteSnippetsAsync(new WebsiteId(websiteId));
				var projection = projector.Project<Snippet, SnippetProjection>(snippets);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapGet("{snippetId:Guid}/details", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsiteSnippetManager manager,
			[FromServices] Projector projector,
			[FromRoute] Guid snippetId) => {
				var snippet = await manager.GetWebsiteSnippetAsync(new WebsiteId(websiteId), new SnippetId(snippetId));
				var projection = projector.Project<Snippet, SnippetEditProjection>(snippet);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapPut("{snippetId:Guid}/edit", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsiteSnippetManager manager,
			[FromServices] Projector projector,
			[FromRoute] Guid snippetId,
			[FromBody] WebsiteSnippetEditRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				request.SnippetId = new SnippetId(snippetId);
				var snippet = await manager.EditSnippetAsync(request);
				var projection = projector.Project<Snippet, SnippetEditProjection>(snippet);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsiteSnippetEditRequestModel>();

		routes.MapDelete("{snippetId:Guid}/delete", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsiteSnippetManager manager,
			[FromRoute] Guid snippetId) => {
				await manager.DeleteSnippetAsync(new WebsiteId(websiteId), new SnippetId(snippetId));
				return Results.Ok();
			}).Authorize(UserPolicy.OwnerPrivilege);

	}

}