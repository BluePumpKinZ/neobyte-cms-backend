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

		routes.MapDelete("{pageId}/delete", async (
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