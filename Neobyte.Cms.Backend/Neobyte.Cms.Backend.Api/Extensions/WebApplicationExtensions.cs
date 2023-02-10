using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neobyte.Cms.Backend.Api.Endpoints.Loader;

namespace Neobyte.Cms.Backend.Api.Extensions; 

public static class WebApplicationExtensions {

	public static WebApplication UseApi (this WebApplication app) {

		// endpoints
		var endpointLoader = app.Services.GetRequiredService<ApiEndpointLoader>();
		endpointLoader.LoadEndpoints(app);

		// swagger
		if (app.Environment.IsDevelopment()) {
			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Neobyte Cms Backend API"));
		}

		app.UseCors(x => x
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader()
		);

		return app;
	}

}