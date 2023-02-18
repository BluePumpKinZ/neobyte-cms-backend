using Microsoft.Extensions.Logging;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using Neobyte.Cms.Backend.RemoteHosting.Connections;
using System.Diagnostics;

namespace Neobyte.Cms.Backend.RemoteHosting.Adapters;

internal class RemoteHostingProvider : IRemoteHostingProvider {

	private readonly IEnumerable<IRemoteHostingConnector> _hostingConnectors;
	private readonly HostingConnectorCache _cache;
	private readonly ILogger<RemoteHostingProvider> _logger;
	private readonly ActivitySource _activitySource;

	public RemoteHostingProvider (IEnumerable<IRemoteHostingConnector> hostingConnectors, HostingConnectorCache cache, ILogger<RemoteHostingProvider> logger, ActivitySource activitySource) {
		_hostingConnectors = hostingConnectors;
		_cache = cache;
		_logger = logger;
		_activitySource = activitySource;
	}

	public IRemoteHostingConnector GetConnector (HostingConnection connection) {
		if (_cache.TryGetConnector(connection.Id, out IRemoteHostingConnector? connector)) {
			_logger.LogDebug("Using cached connector for connection {connectionId}", connection.Id);
			connector!.LastConnectionTime = DateTime.UtcNow;
			return connector;
		}

		_logger.LogInformation("Creating new connector for connection {connectionId}", connection.Id);
		connector = CreateConnector(connection);
		_cache.AddConnector(connection.Id, connector);
		return connector;
	}

	private IRemoteHostingConnector CreateConnector (HostingConnection connection) {
		var hostingConnector = _hostingConnectors.Single(hc => hc.CanConnect(connection));
		hostingConnector.Configure(connection);
		return new RemoteHostingConnectorProxy (_activitySource, hostingConnector);
	}

}