using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoreCSharp.Extensions.System.Collections.Generic;
using Neobyte.Cms.Backend.RemoteHosting.Configuration;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Neobyte.Cms.Backend.RemoteHosting.Connections; 

internal class ConnectionDisconnector {

	private readonly RemoteHostingOptions _options;
	private readonly HostingConnectorCache _cache;
	private readonly ILogger<ConnectionDisconnector> _logger;

	public ConnectionDisconnector (IOptions<RemoteHostingOptions> options, HostingConnectorCache cache, ILogger<ConnectionDisconnector> logger) {
		_cache = cache;
		_logger = logger;
		_options = options.Value;
	}

	public void Start () {
		var timer = new Timer(100);
		timer.Elapsed += DisconnectTimedoutConnections;
		timer.Start();
	}

	private void DisconnectTimedoutConnections (object? sender, ElapsedEventArgs e) {
		_cache.GetConnectors()
			.Where(c => c.connector.LastConnectionTime.AddSeconds(_options.ConnectionTimeout) < DateTime.UtcNow)
			.ForEach(c => {
				_cache.RemoveConnector(c.id);
				_logger.LogInformation("Connector for connection {connectionId} closed after {timeout}s of inactivity",
					c.id, (DateTime.UtcNow - c.connector.LastConnectionTime).TotalSeconds );
			});
	}

}