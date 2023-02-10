using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Monitoring.Configuration;

namespace Neobyte.Cms.Backend.Monitoring.Extensions; 

public static class WebApplicationExtensions {

	public static WebApplication UseMonitoring (this WebApplication app) {
		
		return app;
	}

}