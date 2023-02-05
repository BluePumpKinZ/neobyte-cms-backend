using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Identity.Initializers;

namespace Neobyte.Cms.Backend.Identity.Extensions;

public static class WebApplicationExtensions {

	public static WebApplication UseIdentity (this WebApplication app) {

		app.UseAuthentication();
		app.UseAuthorization();

		using var scope = app.Services.CreateScope();
		var services = scope.ServiceProvider;
		var roleInitializer = services.GetRequiredService<RoleInitializer>();
		roleInitializer.InitializeRoles().Wait();
		
		return app;
	}

}