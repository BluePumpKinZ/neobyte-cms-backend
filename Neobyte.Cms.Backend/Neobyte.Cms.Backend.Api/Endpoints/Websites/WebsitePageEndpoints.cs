﻿using Neobyte.Cms.Backend.Api.Extensions;
using Neobyte.Cms.Backend.Core.Websites.Managers;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Endpoints.Websites;

internal class WebsitePageEndpoints : IApiEndpoints {

	public string GroupName => "Website Pages";
	public string Path => "/api/v1/websites/{websiteId:Guid}/pages";
	public bool Authorized => true;

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapPost("add/existing", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsitePageManager manager,
			[FromBody] WebsiteCreatePageRequestModel request) => {
				request.Id = new WebsiteId(websiteId);
				var response = await manager.CreateExistingPageAsync(request);
				if (!response.Success)
					return Results.BadRequest(new { response.Errors });

				return Results.Ok(new { Message = "Page added" });
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsiteCreatePageRequestModel>();

		routes.MapPost("add/empty", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsitePageManager manager,
			[FromBody] WebsiteCreatePageRequestModel request) => {
				request.Id = new WebsiteId(websiteId);
				var response = await manager.CreateEmptyPageAsync(request);
				if (!response.Success)
					return Results.BadRequest(new { response.Errors });

				return Results.Ok(new { Message = "Page added" });
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsiteCreatePageRequestModel>();

		routes.MapGet("", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsitePageManager manager,
			[FromServices] Projector projector) => {
				var pages = await manager.GetPagesByWebsiteId(new WebsiteId(websiteId));
				var projection = projector.Project<Page, PageProjection>(pages);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapGet("{pageId:Guid}", async (
			[FromRoute] Guid websiteId,
			[FromRoute] Guid pageId,
			[FromServices] WebsitePageManager manager,
			[FromServices] Projector projector) => {
				var page = await manager.GetPageByIdAsync(new WebsiteId(websiteId), new PageId(pageId));
				var projection = projector.Project<Page, PageProjection>(page);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapPut("{pageId:Guid}/edit", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsitePageManager manager,
			[FromServices] Projector projector,
			[FromRoute] Guid pageId,
			[FromBody] WebsiteEditPageRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				request.PageId = new PageId(pageId);
				var page = await manager.EditPageAsync(request);
				var projection = projector.Project<Page, PageProjection>(page);
				return Results.Ok(projection);
			}).Authorize(UserPolicy.OwnerPrivilege);

		routes.MapGet("{pageId:Guid}/render", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsitePageManager manager,
			[FromRoute] Guid pageId) => {
				string response = await manager.RenderPageAsync(new WebsiteId(websiteId), new PageId(pageId));
				return Results.Extensions.Html(response);
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapGet("{pageId:Guid}/source", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsitePageManager manager,
			[FromRoute] Guid pageId) => {
				string response = await manager.GetPageSourceAsync(new WebsiteId(websiteId), new PageId(pageId));
				return Results.Ok(response);
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapPut("{pageId:Guid}/publish/render", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsitePageManager manager,
			[FromRoute] Guid pageId,
			[FromBody] WebsitePagePublishRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				request.PageId = new PageId(pageId);
				await manager.PublishPageRender(request);
				return Results.Ok(new { Message = "Page published" });
			}).Authorize(UserPolicy.ClientPrivilege)
			.ValidateBody<WebsitePagePublishRequestModel>();

		routes.MapPut("{pageId:Guid}/publish/source", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsitePageManager manager,
			[FromRoute] Guid pageId,
			[FromBody] WebsitePagePublishRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				request.PageId = new PageId(pageId);
				await manager.PublishPageSource(request);
				return Results.Ok(new { Message = "Page published" });
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsitePagePublishRequestModel>();

		routes.MapDelete("{pageId:Guid}/delete", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsitePageManager manager,
			[FromRoute] Guid pageId) => {
				await manager.DeletePageAsync(new WebsiteId(websiteId), new PageId(pageId));
				return Results.Ok("Page deleted");
			}).Authorize(UserPolicy.OwnerPrivilege);

	}

}