using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Accounts;

namespace Neobyte.Cms.Backend.Core.Extensions; 

public static class WebApplicationExtensions {

	public static WebApplication UseCore(this WebApplication app) {

		using var scope = app.Services.CreateScope();
		var defaultAccountCreator = scope.ServiceProvider.GetRequiredService<DefaultAccountCreator>();
		defaultAccountCreator.CreateDefaultAccount();
		
		return app;
	}

}