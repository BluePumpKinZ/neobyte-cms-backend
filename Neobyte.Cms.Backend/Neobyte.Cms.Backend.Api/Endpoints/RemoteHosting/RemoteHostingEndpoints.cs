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
				var valid = await manager.CheckConnectionAsync(request);
				return Results.Ok(new { Valid = valid });
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<RemoteHostingRequestModel>();

		routes.MapPost("list", async (
			[FromServices] RemoteHostingManager manager,
			[FromBody] RemoteHostingListRequestModel request) => {
				var entries = await manager.ListEntriesAsync(request.Connection, request.Path);
				return Results.Ok(entries.ToArray());
			}).Authorize(UserPolicy.OwnerPrivilege)
			.ValidateBody<RemoteHostingListRequestModel>();

	}

}