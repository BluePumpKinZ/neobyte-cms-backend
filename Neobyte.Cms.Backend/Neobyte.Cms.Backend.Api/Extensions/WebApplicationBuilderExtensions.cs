using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Api.Endpoints;
using Neobyte.Cms.Backend.Api.Endpoints.Identity;
using Neobyte.Cms.Backend.Api.Endpoints.Loader;
using Neobyte.Cms.Backend.Api.Endpoints.Mailing;

namespace Neobyte.Cms.Backend.Api.Extensions; 

public static class WebApplicationBuilderExtensions {

    public static WebApplicationBuilder AddApi (this WebApplicationBuilder builder) {

        // endpoints
        builder.Services.AddSingleton<ApiEndpointLoader>();
        builder.Services.AddSingleton<IApiEndpoints, IdentityAuthenticationEndpoints>();
        builder.Services.AddSingleton<IApiEndpoints, MailingEndpoints>();

		// swagger
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		return builder;
    }

}