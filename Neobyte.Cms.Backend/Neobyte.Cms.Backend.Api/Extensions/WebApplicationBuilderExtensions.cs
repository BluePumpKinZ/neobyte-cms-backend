using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Api.Authorization;
using Neobyte.Cms.Backend.Api.Endpoints;
using Neobyte.Cms.Backend.Api.Endpoints.Accounts;
using Neobyte.Cms.Backend.Api.Endpoints.Identity;
using Neobyte.Cms.Backend.Api.Endpoints.Loader;
using Neobyte.Cms.Backend.Api.Endpoints.Website;

namespace Neobyte.Cms.Backend.Api.Extensions; 

public static class WebApplicationBuilderExtensions {

    public static WebApplicationBuilder AddApi (this WebApplicationBuilder builder) {

        // endpoints
        builder.Services.AddSingleton<ApiEndpointLoader>();
		builder.Services.AddSingleton<IApiEndpoints, AccountsMeEndpoints>();
        builder.Services.AddSingleton<IApiEndpoints, AccountsListEndpoints>();
        builder.Services.AddSingleton<IApiEndpoints, IdentityAuthenticationEndpoints>();
		builder.Services.AddSingleton<IApiEndpoints, WebsiteFilesEndpoints>();

		// projections
		builder.Services.AddSingleton<ProjectionMapperFactory>();
		builder.Services.AddSingleton(sp => sp.GetRequiredService<ProjectionMapperFactory>().CreateMapper());
		builder.Services.AddScoped<Projector>();
		builder.Services.AddSingleton<IProjection, AccountProjection>();

		// principal
		builder.Services.AddHttpContextAccessor();
		builder.Services.AddScoped<HttpContextIdentityPrincipalConverter>();
		builder.Services.AddScoped(sp => {
			var principalConverter = sp.GetRequiredService<HttpContextIdentityPrincipalConverter>();
			return principalConverter.GetPrincipalAsync().Result;
		});

		// swagger
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddCors();

		return builder;
    }

}