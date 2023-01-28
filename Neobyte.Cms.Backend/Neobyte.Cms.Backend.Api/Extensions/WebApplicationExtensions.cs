using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Api.Endpoints.Loader;

namespace Neobyte.Cms.Backend.Api.Extensions; 

public static class WebApplicationExtensions {

	public static WebApplication UseApi (this WebApplication app) {

		var endpointLoader = app.Services.GetRequiredService<ApiEndpointLoader>();
		endpointLoader.LoadEndpoints(app);

		return app;
	}

}