using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Persistence.EF.Initializer;

namespace Neobyte.Cms.Backend.Persistence.Extensions;

public static class WebApplicationExtensions {

	public static WebApplication UsePersistence (this WebApplication app) {
		using var scope = app.Services.CreateScope();
		var initializer = scope.ServiceProvider.GetService<DbContextInitializer>()!;
		initializer.Initialize();
		return app;
	}

}