using MoreCSharp.Extensions.System.Collections.Generic;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using System.Collections.Concurrent;

namespace Neobyte.Cms.Backend.RemoteHosting.Connections;

internal class HostingConnectorCache : IDisposable {

	private readonly ConcurrentDictionary<HostingConnectionId, RemoteHostingConnector> _cache = new();

	public bool AddConnector (HostingConnectionId id, RemoteHostingConnector connector) {
		return _cache.TryAdd(id, connector);
	}

	public bool TryGetConnector (HostingConnectionId id, out RemoteHostingConnector? connector) {
		return _cache.TryGetValue(id, out connector);
	}
	
	public IEnumerable<(HostingConnectionId id, RemoteHostingConnector connector)> GetConnectors () {
		return _cache.Select(kvp => (kvp.Key, kvp.Value));
	}

	public bool RemoveConnector (HostingConnectionId id) {
		bool success = _cache.TryRemove(id, out var connector);
		connector?.Dispose();
		return success;
	}

	public void Dispose () {
		Clear();
	}

	private void Clear () {
		_cache.Keys.ForEach(c => RemoveConnector(c));
	}

}