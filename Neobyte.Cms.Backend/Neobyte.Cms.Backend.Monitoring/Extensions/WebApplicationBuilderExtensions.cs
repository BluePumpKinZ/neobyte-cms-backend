using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neobyte.Cms.Backend.Core.Ports.Monitoring;
using Neobyte.Cms.Backend.Monitoring.Adapters;
using Neobyte.Cms.Backend.Monitoring.Configuration;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Diagnostics;

namespace Neobyte.Cms.Backend.Monitoring.Extensions;

public static class WebApplicationBuilderExtensions {
	public static WebApplicationBuilder AddMonitoring (this WebApplicationBuilder builder) {

		builder.Services.Configure<MonitoringOptions>(builder.Configuration.GetSection(MonitoringOptions.SectionName));
		var options = new MonitoringOptions();
		builder.Configuration.GetSection(MonitoringOptions.SectionName).Bind(options);

		// Add logging
		var logLevel = builder.Configuration.GetValue<LogEventLevel>("Logging:LogLevel:Default");
		var logLevels = builder.Configuration.GetSection("Logging:LogLevel").GetChildren();

		var loggerConfiguration = new LoggerConfiguration()
			.Enrich.FromLogContext()
			.Enrich.WithCorrelationId()
			.MinimumLevel.ControlledBy(new LoggingLevelSwitch(logLevel))
			.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {CorrelationId} {Level:u3}] {Message:lj}{NewLine}{Exception}");

		foreach (var level in logLevels) {
			loggerConfiguration.MinimumLevel.Override(level.Key, Enum.Parse<LogEventLevel>(level.Value!));
		}

		var logger = loggerConfiguration.CreateLogger();

		builder.Logging.ClearProviders();
		builder.Host.UseSerilog(logger);

		// Add Tracing
		builder.Services.AddRouting();
		builder.Services.AddReverseProxy()
			.LoadFromMemory(options.GetRoutes(), options.GetClusters());

		builder.Services.AddOpenTelemetry()
			.WithTracing(config => config
				.SetSampler(new TraceIdRatioBasedSampler(options.Propability))
				.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(options.ServiceName))
				.AddSqlClientInstrumentation(opt => {
					opt.SetDbStatementForText = true;
					opt.RecordException = true;
				})
				.AddAspNetCoreInstrumentation(opt => {
					opt.EnableGrpcAspNetCoreSupport = true;
					opt.RecordException = true;
					opt.Filter = httpContext => {
						var path = httpContext.Request.Path.Value!;
						if (path.StartsWith("/api/v1/tracing"))
							return false;
						if (path.StartsWith("/api/v1/metrics"))
							return false;
						return true;
					};
				})
				.AddHttpClientInstrumentation(opt => {
					opt.RecordException = true;
				})
				.AddOtlpExporter(otlpOptions => {
					otlpOptions.Endpoint = new Uri($"http://{options.JaegerHost}:{options.JaegerPort}");
				})
			);

		builder.Services.AddSingleton(new ActivitySource(options.ServiceName));

		// Add metrics
		builder.Services.AddSingleton<IMetricsStore, MetricsStore>();

		return builder;
	}
}