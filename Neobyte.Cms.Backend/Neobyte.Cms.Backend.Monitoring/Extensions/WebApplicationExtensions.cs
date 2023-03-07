﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Monitoring.Configuration;
using Prometheus;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Configuration;

namespace Neobyte.Cms.Backend.Monitoring.Extensions;

public static class WebApplicationExtensions {

	public static WebApplication UseMonitoring (this WebApplication app) {

		var options = app.Services.GetRequiredService<IOptions<MonitoringOptions>>().Value;

		app.UseRouting();

		app.Map("/", context => {
			context.RequestServices.GetRequiredService<InMemoryConfigProvider>()
			.Update(options.GetRoutes(), options.GetClusters());
			return Task.CompletedTask;
		});

		app.MapReverseProxy();

		if (app.Environment.EnvironmentName == "Testing")
			return app;

		app.UseHttpMetrics();

		var metricsServer = new MetricServer(options.Metrics.Host, options.Metrics.Port, "api/v1/metrics/");
		metricsServer.Start();

		return app;
	}

}