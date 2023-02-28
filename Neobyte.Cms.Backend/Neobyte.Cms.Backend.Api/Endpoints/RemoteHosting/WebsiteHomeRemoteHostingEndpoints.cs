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
			[FromServices] HomeRemoteHostingManager manager,
			[FromBody] WebsiteCreateRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				await manager.HomeAddFolderAsync(request);
				return Results.Ok(new { Message = "Created" });
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsiteCreateRequestModel>();

		routes.MapGet("folder/list", async (
			[FromRoute] Guid websiteId,
			[FromServices] HomeRemoteHostingManager manager,
			[FromQuery] string path) => {
				var request = new WebsiteListRequestModel {
					WebsiteId = new WebsiteId(websiteId), Path = path
				};
				var entries = await manager.HomeListEntriesAsync(request);
				return Results.Ok(entries.ToArray());
			}).Authorize(UserPolicy.OwnerPrivilege);

		routes.MapPut("folder/rename", async (
			[FromRoute] Guid websiteId,
			[FromServices] HomeRemoteHostingManager manager,
			[FromBody] WebsiteRenameRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				await manager.HomeRenameFolderAsync(request);
				return Results.Ok(new { Message = "Renamed" });
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsiteRenameRequestModel>();

		routes.MapDelete("folder/delete", async (
			[FromRoute] Guid websiteId,
			[FromServices] HomeRemoteHostingManager manager,
			[FromQuery] string path) => {
				var request = new WebsiteDeleteRequestModel {
					WebsiteId = new WebsiteId(websiteId), Path = path
				};
				await manager.HomeDeleteFolderAsync(request);
				return Results.Ok(new { Message = "Deleted" });
			}).Authorize(UserPolicy.OwnerPrivilege);

		routes.MapPut("file/rename", async ([FromRoute] Guid websiteId,
			[FromServices] HomeRemoteHostingManager manager,
			[FromBody] WebsiteRenameRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				await manager.HomeRenameFileAsync(request);
				return Results.Ok(new { Message = "Renamed" });
			}).Authorize(UserPolicy.OwnerPrivilege)
		.ValidateBody<WebsiteRenameRequestModel>();

		routes.MapDelete("file/delete", async (
			[FromRoute] Guid websiteId,
			[FromServices] HomeRemoteHostingManager manager,
			[FromQuery] string path) => {
				var request = new WebsiteDeleteRequestModel {
					WebsiteId = new WebsiteId(websiteId), Path = path
				};
				await manager.HomeDeleteFileAsync(request);
				return Results.Ok(new { Message = "Deleted" });
			}).Authorize(UserPolicy.OwnerPrivilege);

	}

}