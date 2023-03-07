using MoreCSharp.Extensions.System.Collections.Generic;
using Neobyte.Cms.Backend.Core.Ports.Monitoring;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using System.Diagnostics.CodeAnalysis;

namespace Neobyte.Cms.Backend.RemoteHosting.Connections;

internal class HostingConnectorCache : IDisposable {

	private readonly HashSet<HostingConnectorCacheEntry> _cache = new();
	private readonly IMetricsStore _metrics;

	public HostingConnectorCache (IMetricsStore metrics) {
		_metrics = metrics;
	}

	public bool AddConnector (HostingConnection connection, IRemoteHostingConnector connector) {
		bool succes = _cache.Add(new HostingConnectorCacheEntry(connection, connector));
		if (succes)
			_metrics.ActiveRemoteHostingConnections.Inc();
		return succes;
	}

	public bool TryGetConnector (HostingConnection connection, [MaybeNullWhen(false)] out IRemoteHostingConnector connector) {
		if (_cache.TryGetValue(new HostingConnectorCacheEntry(connection, null), out var entry)) {
			connector = entry.Connector!;
			return true;
		}
		connector = null;
		return false;
	}

	public IEnumerable<(HostingConnection connection, IRemoteHostingConnector connector)> GetConnectors () {
		return _cache.Select(kvp => (kvp.Connection, kvp.Connector!));
	}

	public bool RemoveConnector (HostingConnection connection) {
		if (!TryGetConnector(connection, out var connector))
			return false;

		bool success = _cache.Remove(new HostingConnectorCacheEntry(connection, null));
		connector.Disconnect();
		if (success)
			_metrics.ActiveRemoteHostingConnections.Dec();
		return success;
	}

	public void Dispose () {
		Clear();
	}

	private void Clear () {
		_cache.Select(e => e.Connection).ForEach(c => RemoveConnector(c));
	}

}