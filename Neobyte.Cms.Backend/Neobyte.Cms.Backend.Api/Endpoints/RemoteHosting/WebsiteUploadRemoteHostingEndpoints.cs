using Neobyte.Cms.Backend.Core.RemoteHosting.Managers;
using Neobyte.Cms.Backend.Core.RemoteHosting.Models;
using Neobyte.Cms.Backend.Domain.Websites;
using System.Linq;

namespace Neobyte.Cms.Backend.Api.Endpoints.RemoteHosting;

public class WebsiteUploadRemoteHostingEndpoints : IApiEndpoints {

	public string GroupName => "Website Upload Remote Hosting";
	public string Path => "/api/v1/websites/{websiteId:Guid}/upload";
	public bool Authorized => true;

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapPost("folder/create", async (
			[FromRoute] Guid websiteId,
			[FromServices] RemoteHostingManager manager,
			[FromBody] WebsiteCreateFolderRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				await manager.UploadAddFolderAsync(request);
				return Results.Ok(new { Message = "Created" });
			}).Authorize(UserPolicy.ClientPrivilege)
			.ValidateBody<WebsiteCreateFolderRequestModel>();

		routes.MapGet("folder/list", async (
			[FromRoute] Guid websiteId,
			[FromServices] RemoteHostingManager manager,
			[FromQuery] string path) => {
				var request = new WebsiteListRequestModel {
					WebsiteId = new WebsiteId(websiteId), Path = path
				};
				var entries = await manager.UploadListEntriesAsync(request);
				return Results.Ok(entries.ToArray());
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapPut("folder/rename", async (
			[FromRoute] Guid websiteId,
			[FromServices] RemoteHostingManager manager,
			[FromBody] WebsiteRenameFolderRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				await manager.UploadRenameFolderAsync(request);
				return Results.Ok(new { Message = "Renamed" });
			}).Authorize(UserPolicy.ClientPrivilege)
			.ValidateBody<WebsiteRenameFolderRequestModel>();

		routes.MapDelete("folder/delete", async (
			[FromRoute] Guid websiteId,
			[FromServices] RemoteHostingManager manager,
			[FromQuery] string path) => {
				var request = new WebsiteDeleteFolderRequestModel {
					WebsiteId = new WebsiteId(websiteId), Path = path
				};
				await manager.UploadDeleteFolderAsync(request);
				return Results.Ok(new { Message = "Deleted" });
			}).Authorize(UserPolicy.ClientPrivilege);

	}

}