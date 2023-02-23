using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Api.Endpoints;
using Neobyte.Cms.Backend.Api.Endpoints.Accounts;
using Neobyte.Cms.Backend.Api.Endpoints.Identity;
using Neobyte.Cms.Backend.Api.Endpoints.Loader;
using Neobyte.Cms.Backend.Api.Endpoints.Mailing;
using Neobyte.Cms.Backend.Api.Endpoints.RemoteHosting;
using Neobyte.Cms.Backend.Api.Endpoints.Websites;

namespace Neobyte.Cms.Backend.Api.Extensions;

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddApi (this WebApplicationBuilder builder) {

		// endpoints
		builder.Services.AddSingleton<ApiEndpointLoader>();
		builder.Services.AddSingleton<IApiEndpoints, AccountsMeEndpoints>();
		builder.Services.AddSingleton<IApiEndpoints, AccountsListEndpoints>();
		builder.Services.AddSingleton<IApiEndpoints, IdentityAuthenticationEndpoints>();
		builder.Services.AddSingleton<IApiEndpoints, MailingEndpoints>();
		builder.Services.AddSingleton<IApiEndpoints, WebsiteEndpoints>();
		builder.Services.AddSingleton<IApiEndpoints, RemoteHostingEndpoints>();
		builder.Services.AddSingleton<IApiEndpoints, WebsitePageEndpoints>();
		builder.Services.AddSingleton<IApiEndpoints, WebsiteRemoteHostingEndpoints>();
		builder.Services.AddSingleton<IApiEndpoints, WebsiteSnippetEndpoints>();
		builder.Services.AddSingleton<IApiEndpoints, WebsiteUserEndpoints>();

		// projections
		builder.Services.AddSingleton<ProjectionMapperFactory>();
		builder.Services.AddSingleton(sp => sp.GetRequiredService<ProjectionMapperFactory>().CreateMapper());
		builder.Services.AddScoped<Projector>();
		builder.Services.AddSingleton<IProjection, AccountProjection>();
		builder.Services.AddSingleton<IProjection, PageProjection>();
		builder.Services.AddSingleton<IProjection, SnippetEditProjection>();
		builder.Services.AddSingleton<IProjection, SnippetProjection>();
		builder.Services.AddSingleton<IProjection, WebsiteEditProjection>();
		builder.Services.AddSingleton<IProjection, WebsiteProjection>();

		// principal
		builder.Services.AddHttpContextAccessor();
		builder.Services.AddScoped<HttpContextIdentityPrincipalConverter>();
		builder.Services.AddScoped(sp => {
			var principalConverter = sp.GetRequiredService<HttpContextIdentityPrincipalConverter>();
			return principalConverter.GetPrincipalAsync().Result;
		});

		// authorization
		builder.Services.AddScoped<EnabledAccountFilter>();

		// swagger
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddCors();

		return builder;
	}

}