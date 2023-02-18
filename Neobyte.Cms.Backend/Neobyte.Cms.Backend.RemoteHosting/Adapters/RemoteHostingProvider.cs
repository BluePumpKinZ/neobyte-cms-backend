using Microsoft.Extensions.Logging;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using Neobyte.Cms.Backend.RemoteHosting.Connections;
using Neobyte.Cms.Backend.Utils;

namespace Neobyte.Cms.Backend.RemoteHosting.Adapters; 

internal class RemoteHostingProvider : IRemoteHostingProvider {

	private readonly IEnumerable<IRemoteHostingConnector> _hostingConnectors;
	private readonly HostingConnectorCache _cache;
	private readonly ILogger<RemoteHostingProvider> _logger;

	public RemoteHostingProvider (IEnumerable<IRemoteHostingConnector> hostingConnectors, HostingConnectorCache cache, ILogger<RemoteHostingProvider> logger) {
		_hostingConnectors = hostingConnectors;
		_cache = cache;
		_logger = logger;
	}

	public IRemoteHostingConnector GetConnector (HostingConnection connection) {
		if (_cache.TryGetConnector(connection.Id, out IRemoteHostingConnector? connector)) {
			_logger.LogDebug("Using cached connector for connection {connectionId}", connection.Id);
			connector!.LastConnectionTime = DateTime.UtcNow;
			return connector;
		}

		_logger.LogDebug("Creating new connector for connection {connectionId}", connection.Id);
		connector = CreateConnectory(connection);
		_cache.AddConnector(connection.Id, connector);
		return connector;
	}

	private IRemoteHostingConnector CreateConnectory (HostingConnection connection) {
		var hostingConnector = _hostingConnectors.Single(hc => hc.CanConnect(connection));
		hostingConnector.Configure(connection);
		return hostingConnector;
	}

}