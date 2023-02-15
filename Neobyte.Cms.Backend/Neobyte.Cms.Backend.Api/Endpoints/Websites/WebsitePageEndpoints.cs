using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Websites.Managers;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Endpoints.Websites;

public class WebsitePageEndpoints : IApiEndpoints {

	public string GroupName => "Website Pages";
	public string Path => "/api/v1/websites/{websiteId:Guid}/pages";

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapPost("add/existing", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsitePageManager manager,
			[FromBody] WebsiteCreatePageRequestModel request) => {
				try {
					request.Id = new WebsiteId(websiteId);
					var response = await manager.CreateExistingPageAsync(request);
					if (!response.Success)
						return Results.BadRequest(new { response.Errors });
				} catch (ApplicationException e) {
					return Results.BadRequest(new { e.Message });
				}

				return Results.Ok(new { Message = "Page added" });
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsiteCreatePageRequestModel>();

		routes.MapGet("{pageId:Guid}/render", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsitePageManager manager,
			[FromRoute] Guid pageId,
			[FromServices] IHttpContextAccessor httpContextAccessor) => {
				string response;
				try {
					response = await manager.RenderPageAsync(new WebsiteId(websiteId), new PageId(pageId));
				} catch (NotFoundException e) {
					return Results.NotFound(new { e.Message });
				} catch (ApplicationException e) {
					return Results.BadRequest(new { e.Message });
				}

				var httpContext = httpContextAccessor.HttpContext!;

				httpContext.Response.Headers.Remove("Content-Type");
				httpContext.Response.Headers.Add("Content-Type", "text/html");
				return Results.Ok(response);
			}).Authorize(UserPolicy.OwnerPrivilege);

		routes.MapDelete("{pageId:Guid}/delete", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsitePageManager manager,
			[FromRoute] Guid pageId) => {
				try {
					await manager.DeletePageAsync(new WebsiteId(websiteId), new PageId(pageId));
				} catch (NotFoundException) {
					return Results.NotFound();
				} catch (ApplicationException e) {
					return Results.BadRequest(new { e.Message });
				}
				return Results.Ok("Page deleted");
			}).Authorize(UserPolicy.OwnerPrivilege);

	}

}