using Neobyte.Cms.Backend.Core.RemoteHosting.Managers;
using Neobyte.Cms.Backend.Core.RemoteHosting.Models;
using System.Linq;

namespace Neobyte.Cms.Backend.Api.Endpoints.RemoteHosting;

public class RemoteHostingEndpoints : IApiEndpoints {

	public string GroupName => "Hosting Connection";
	public string Path => "/api/v1/remote-hosting";
	public bool Authorized => true;

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapGet("verify", async (
			[FromServices] RemoteHostingManager manager,
			[FromBody] RemoteHostingRequestModel request) => {
				var valid = await manager.PublicCheckConnectionAsync(request);
				return Results.Ok(new { Valid = valid });
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<RemoteHostingRequestModel>();

		routes.MapPost("folder/add", async (
			[FromServices] RemoteHostingManager manager,
			[FromBody] RemoteHostingAddFolderRequestModel request) => {
				await manager.PublicAddFolderAsync(request);
				return Results.Ok("Added");
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<RemoteHostingAddFolderRequestModel>();

		routes.MapPost("folder/list", async (
			[FromServices] RemoteHostingManager manager,
			[FromBody] RemoteHostingListRequestModel request) => {
				var entries = await manager.PublicListEntriesAsync(request);
				return Results.Ok(entries.ToArray());
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<RemoteHostingListRequestModel>();

		routes.MapPost("folder/rename", async (
			[FromServices] RemoteHostingManager manager,
			[FromBody] RemoteHostingRenameRequestModel request) => {
				await manager.PublicRenameFolderAsync(request);
				return Results.Ok("Renamed");
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<RemoteHostingRenameRequestModel>();

	}

}