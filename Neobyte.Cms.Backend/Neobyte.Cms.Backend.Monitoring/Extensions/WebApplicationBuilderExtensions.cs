using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
		// Add logging
		var logLevel = builder.Configuration.GetValue<LogEventLevel>("Logging:LogLevel:Default");
		var logLevels = builder.Configuration.GetSection("Logging:LogLevel").GetChildren();

		var loggerConfiguration = new LoggerConfiguration()
			.Enrich.FromLogContext()
			.MinimumLevel.ControlledBy(new LoggingLevelSwitch(logLevel))
			.WriteTo.Console();

		foreach (var level in logLevels) {
			loggerConfiguration.MinimumLevel.Override(level.Key, Enum.Parse<LogEventLevel>(level.Value!));
		}

		var logger = loggerConfiguration.CreateLogger();

		builder.Logging.ClearProviders();
		builder.Logging.AddSerilog(logger);

		// Add monitoring
		builder.Services.Configure<MonitoringOptions>(builder.Configuration.GetSection(MonitoringOptions.SectionName));
		var monitoringOptions = new MonitoringOptions();
		builder.Configuration.GetSection(MonitoringOptions.SectionName).Bind(monitoringOptions);

		builder.Services.AddRouting();
		builder.Services.AddReverseProxy()
			.LoadFromMemory(monitoringOptions.GetRoutes(), monitoringOptions.GetClusters());

		builder.Services.AddOpenTelemetry()
			.WithTracing(config => config
				.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(monitoringOptions.ServiceName))
				.AddSqlClientInstrumentation(opt => {
					opt.SetDbStatementForText = true;
					opt.RecordException = true;
				})
				.AddAspNetCoreInstrumentation(opt => {
					opt.EnableGrpcAspNetCoreSupport = true;
					opt.RecordException = true;
				})
				.AddHttpClientInstrumentation(opt => {
					opt.RecordException = true;
				})
				.AddOtlpExporter(otlpOptions => {
					otlpOptions.Endpoint = new Uri($"http://{monitoringOptions.JaegerHost}:{monitoringOptions.JaegerPort}");
				})
			);

		builder.Services.AddSingleton(new ActivitySource(monitoringOptions.ServiceName));

		return builder;
	}
}