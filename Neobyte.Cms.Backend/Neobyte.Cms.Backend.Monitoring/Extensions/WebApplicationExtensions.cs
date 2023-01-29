using Microsoft.AspNetCore.Builder;

namespace Neobyte.Cms.Backend.Monitoring.Extensions; 

public static class WebApplicationExtensions {

	public static WebApplication UseMonitoring (this WebApplication app) {

		return app;
	}

}