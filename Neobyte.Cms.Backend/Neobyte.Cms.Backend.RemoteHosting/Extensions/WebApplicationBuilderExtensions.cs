using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.RemoteHosting.Adapters;
using Neobyte.Cms.Backend.RemoteHosting.Connectors;

namespace Neobyte.Cms.Backend.RemoteHosting.Extensions; 

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddRemoteHosting(this WebApplicationBuilder builder) {

		builder.Services.AddScoped<IRemoteHostingProvider, RemoteHostingProvider>();

		// builder.Services.AddScoped<IRemoteHostingConnector, FtpConnector>();
		builder.Services.AddScoped<IRemoteHostingConnector, FluentFtpConnector>();
		
		return builder;
	}

}