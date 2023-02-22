using Neobyte.Cms.Backend.Core.RemoteHosting.Managers;
using Neobyte.Cms.Backend.Core.RemoteHosting.Models;
using Neobyte.Cms.Backend.Domain.Websites;
using System.Linq;

namespace Neobyte.Cms.Backend.Api.Endpoints.RemoteHosting;

public class WebsiteHomeRemoteHostingEndpoints : IApiEndpoints {

	public string GroupName => "Website Home Remote Hosting";
	public string Path => "/api/v1/websites/{websiteId:Guid}/home";
	public bool Authorized => true;

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapPost("folder/create", async (
			[FromRoute] Guid websiteId,
			[FromServices] RemoteHostingManager manager,
			[FromBody] WebsiteCreateFolderRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				await manager.HomeAddFolderAsync(request);
				return Results.Ok("Created");
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsiteCreateFolderRequestModel>();

		routes.MapGet("folder/list", async (
			[FromRoute] Guid websiteId,
			[FromServices] RemoteHostingManager manager,
			[FromQuery] string path) => {
				var request = new WebsiteListRequestModel {
					WebsiteId = new WebsiteId(websiteId), Path = path
				};
				var entries = await manager.HomeListEntriesAsync(request);
				return Results.Ok(entries.ToArray());
			}).Authorize(UserPolicy.OwnerPrivilege);

		routes.MapPut("folder/rename", async (
			[FromRoute] Guid websiteId,
			[FromServices] RemoteHostingManager manager,
			[FromBody] WebsiteRenameFolderRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				await manager.HomeRenameFolderAsync(request);
				return Results.Ok("Renamed");
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsiteRenameFolderRequestModel>();

		routes.MapDelete("folder/delete", async (
			[FromRoute] Guid websiteId,
			[FromServices] RemoteHostingManager manager,
			[FromQuery] string path) => {
				var request = new WebsiteDeleteFolderRequestModel {
					WebsiteId = new WebsiteId(websiteId), Path = path
				};
				await manager.HomeDeleteFolderAsync(request);
				return Results.Ok("Deleted");
			}).Authorize(UserPolicy.OwnerPrivilege);

	}

}