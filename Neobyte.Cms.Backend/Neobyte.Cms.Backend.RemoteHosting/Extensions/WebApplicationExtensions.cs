using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.RemoteHosting.Connections;

namespace Neobyte.Cms.Backend.RemoteHosting.Extensions; 

public static class WebApplicationExtensions {

	public static WebApplication UseRemoteHosting (this WebApplication app) {
		var disconnector = app.Services.GetRequiredService<ConnectionDisconnector>();
		// disconnector.Start();
		return app;
	}

}