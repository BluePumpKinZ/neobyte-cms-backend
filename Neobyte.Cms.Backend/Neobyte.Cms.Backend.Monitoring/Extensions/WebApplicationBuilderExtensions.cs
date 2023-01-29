using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Core;
using System;
using Microsoft.Extensions.Logging;

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
		

		// Add tracing
		// Have fun arne

		return builder;
    }

}