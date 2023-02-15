using Neobyte.Cms.Backend.Core.Websites.Managers;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Endpoints.Websites;

public class WebsitePageEndpoints : IApiEndpoints {

	public string GroupName => "Website Pages";
	public string Path => "/api/v1/websites/{id:Guid}/pages";

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapPost("add/existing", async (
			[FromRoute] Guid id,
			[FromServices] WebsitePageManager manager,
			[FromBody] WebsiteCreatePageRequestModel request,
			[FromServices] Principal principal) => {
				var websiteId = new WebsiteId(id);
				request.Id = websiteId;
				var response = await manager.CreateExistingPageAsync(request);
				if (!response.Success)
					return Results.BadRequest(new { response.Errors });

				return Results.Ok(new { Message = "Page added" });
			}).Authorize(UserPolicy.OwnerPrivilege);

	}

}