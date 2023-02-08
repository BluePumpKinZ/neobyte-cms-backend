using Microsoft.AspNetCore.Http;
using Neobyte.Cms.Backend.Core.Mailing.Managers;
using Neobyte.Cms.Backend.Core.Mailing.Models;
using System.Diagnostics;

namespace Neobyte.Cms.Backend.Api.Endpoints.Mailing;

internal class MailingEndpoints : IApiEndpoints {
	public ActivitySource MyActivitySource = new("Neobyte.Cms.Backend");

	public string GroupName => "Mailing";

	public string Path => "/api/v1/mailing";

	public void RegisterApis (RouteGroupBuilder routes) {
		routes.MapPost("sendtest",
			async ([FromServices] MailingManager manager, [FromQuery(Name = "emailTo")] string emailTo) => {
				using var activity = MyActivitySource.StartActivity("sendTestEmail");
				activity?.SetTag("foo", 1);
				activity?.SetTag("bar", "Hello, World!");
				activity?.SetTag("baz", new int[] {1, 2, 3});
				await manager.SendEmail(new MailingSendEmailRequestModel(emailTo, "Testmail Neobyte",
					"Ulle dikke moe"));
				return Results.Ok();
			});
		routes.MapGet("hello", () => {
			using var activity = MyActivitySource.StartActivity("SayHello");
			activity?.SetTag("foo", 1);
			activity?.SetTag("bar", "Hello, World!");
			activity?.SetTag("baz", new int[] {1, 2, 3});

			return "Hello, World!";
		});
	}
}