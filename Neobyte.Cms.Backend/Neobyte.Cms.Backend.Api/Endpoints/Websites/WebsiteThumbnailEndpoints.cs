using Neobyte.Cms.Backend.Core.Websites.Managers;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Endpoints.Websites;

public class WebsiteThumbnailEndpoints : IApiEndpoints {

	public string GroupName => "Website Thumbnails";
	public string Path => "api/v1/websites/{websiteId:Guid}/thumbnails";
	public bool Authorized => false;

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapGet("", async (
			[FromRoute] Guid websiteId,
			[FromServices] WebsiteThumbnailManager manager) => {
				var bytes = await manager.GetWebsiteThumbnailAsync(new WebsiteId(websiteId));
				return Results.File(bytes, "image/png");
		});

	}

}