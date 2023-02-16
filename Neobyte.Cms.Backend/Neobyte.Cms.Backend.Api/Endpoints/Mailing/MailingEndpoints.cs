using Neobyte.Cms.Backend.Core.Mailing.Managers;
using Neobyte.Cms.Backend.Core.Mailing.Models;
using System.Diagnostics;

namespace Neobyte.Cms.Backend.Api.Endpoints.Mailing;

internal class MailingEndpoints : IApiEndpoints {

	public string GroupName => "Mailing";
	public string Path => "/api/v1/mailing";
	public bool Authorized => false;

	public void RegisterApis (RouteGroupBuilder routes) {
		routes.MapPost("sendtest", async (
				[FromServices] MailingManager manager,
				[FromServices] ActivitySource activitySource,
				[FromQuery(Name = "emailTo")] string emailTo) => {
				using var activity = activitySource.StartActivity();
				activity?.SetTag("foo", 1);
				activity?.SetTag("bar", "Hello, World!");
				activity?.SetTag("baz", new int[] { 1, 2, 3 });
				await manager.SendEmail(new MailingSendEmailRequestModel(emailTo, "Testmail Neobyte",
					"Ulle dikke moe"));
				return Results.Ok();
			});
		routes.MapGet("hello", ([FromServices] ActivitySource activitySource) => {
			using var activity = activitySource.StartActivity();
			activity?.SetTag("foo", 1);
			activity?.SetTag("bar", "Hello, World!");
			activity?.SetTag("baz", new int[] { 1, 2, 3 });
			return "Hello, World!";
		});
	}
}