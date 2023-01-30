using Microsoft.AspNetCore.Http;
using Neobyte.Cms.Backend.Core.Mailing.Managers;
using Neobyte.Cms.Backend.Core.Mailing.Models;

namespace Neobyte.Cms.Backend.Api.Endpoints.Mailing;

internal class MailingEndpoints : IApiEndpoints {
    public string GroupName => "Mailing";

    public string Path => "/api/v1/mailing";
    
    public void RegisterApis(RouteGroupBuilder routes) {
        routes.MapPost("sendtest", async ([FromServices] MailingManager manager, [FromQuery(Name = "emailTo")] string emailTo) => {
            await manager.SendEmail(new MailingSendEmailRequestModel(emailTo, "Testmail Neobyte", "Ulle dikke moe"));
            return Results.Ok();

        });
    }
}