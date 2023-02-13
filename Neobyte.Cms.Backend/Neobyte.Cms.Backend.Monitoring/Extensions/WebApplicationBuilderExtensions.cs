using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Core;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Monitoring.Configuration;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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

		builder.Services.Configure<MonitoringOptions>(builder.Configuration.GetSection(MonitoringOptions.SectionName));
		var monitoringOptions = new MonitoringOptions();
		builder.Configuration.GetSection(MonitoringOptions.SectionName).Bind(monitoringOptions);
		var serviceName = monitoringOptions.ServiceName;

		builder.Services.AddOpenTelemetry()
			.WithTracing(config => config
				.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
				.AddConsoleExporter()
				.AddSqlClientInstrumentation(opt => {
					opt.SetDbStatementForText = true;
					opt.RecordException = true;
				})
				.AddAspNetCoreInstrumentation()
				.AddHttpClientInstrumentation()
				.AddOtlpExporter(otlpOptions => {
					otlpOptions.Endpoint = new Uri("http://localhost:4317");
				})
			);


		return builder;
	}
}