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
			[FromServices] UploadRemoteHostingManager manager,
			[FromBody] WebsiteCreateRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				await manager.UploadAddFolderAsync(request);
				return Results.Ok(new { Message = "Created" });
			}).Authorize(UserPolicy.ClientPrivilege)
			.ValidateBody<WebsiteCreateRequestModel>();

		routes.MapGet("folder/list", async (
			[FromRoute] Guid websiteId,
			[FromServices] UploadRemoteHostingManager manager,
			[FromQuery] string path) => {
				var request = new WebsiteListRequestModel {
					WebsiteId = new WebsiteId(websiteId), Path = path
				};
				var entries = await manager.UploadListEntriesAsync(request);
				return Results.Ok(entries.ToArray());
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapPut("folder/rename", async (
			[FromRoute] Guid websiteId,
			[FromServices] UploadRemoteHostingManager manager,
			[FromBody] WebsiteRenameRequestModel request) => {
				request.WebsiteId = new WebsiteId(websiteId);
				await manager.UploadRenameFolderAsync(request);
				return Results.Ok(new { Message = "Renamed" });
			}).Authorize(UserPolicy.ClientPrivilege)
			.ValidateBody<WebsiteRenameRequestModel>();

		routes.MapDelete("folder/delete", async (
			[FromRoute] Guid websiteId,
			[FromServices] UploadRemoteHostingManager manager,
			[FromQuery] string path) => {
				var request = new WebsiteDeleteRequestModel {
					WebsiteId = new WebsiteId(websiteId), Path = path
				};
				await manager.UploadDeleteFolderAsync(request);
				return Results.Ok(new { Message = "Deleted" });
			}).Authorize(UserPolicy.ClientPrivilege);

		routes.MapPut("file/rename", async ([FromRoute] Guid websiteId,
			[FromServices] UploadRemoteHostingManager manager,
			[FromQuery] string path, string newPath) => {
				var request = new WebsiteRenameRequestModel {
					WebsiteId = new WebsiteId(websiteId), Path = path, NewPath = newPath
				};
				await manager.UploadRenameFileAsync(request);
				return Results.Ok(new { Message = "Renamed" });
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<WebsiteRenameRequestModel>();

		routes.MapDelete("file/delete", async (
			[FromRoute] Guid websiteId,
			[FromServices] UploadRemoteHostingManager manager,
			[FromQuery] string path) => {
				var request = new WebsiteDeleteRequestModel {
					WebsiteId = new WebsiteId(websiteId), Path = path
				};
				await manager.UploadDeleteFileAsync(request);
				return Results.Ok(new { Message = "Deleted" });
			}).Authorize(UserPolicy.OwnerPrivilege);

	}

}