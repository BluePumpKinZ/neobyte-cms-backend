using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.RemoteHosting.Adapters;
using Neobyte.Cms.Backend.RemoteHosting.Configuration;
using Neobyte.Cms.Backend.RemoteHosting.Connections;
using Neobyte.Cms.Backend.RemoteHosting.Connections.Connectors;

namespace Neobyte.Cms.Backend.RemoteHosting.Extensions;

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddRemoteHosting(this WebApplicationBuilder builder) {

		builder.Services.Configure<RemoteHostingOptions>(builder.Configuration.GetSection(RemoteHostingOptions.Section));

		builder.Services.AddScoped<IRemoteHostingProvider, RemoteHostingProvider>();
		builder.Services.AddSingleton<HostingConnectorCache>();
		builder.Services.AddSingleton<ConnectionDisconnector>();
		
		builder.Services.AddScoped<IRemoteHostingConnector, FluentFtpConnector>();

		return builder;
	}

}