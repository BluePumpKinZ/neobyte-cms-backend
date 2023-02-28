using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Monitoring.Configuration;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Configuration;

namespace Neobyte.Cms.Backend.Monitoring.Extensions;

public static class WebApplicationExtensions {

	public static WebApplication UseMonitoring (this WebApplication app) {

		var options = app.Services.GetRequiredService<IOptions<MonitoringOptions>>().Value;

		app.UseRouting();

		app.Map("/api/v1/monitoring/dashboard", context => {
			context.RequestServices.GetRequiredService<InMemoryConfigProvider>()
			.Update(options.Dashboard.GetRoutes(), options.Dashboard.GetClusters());
			return Task.CompletedTask;
		});

		app.MapReverseProxy();
		return app;
	}

}