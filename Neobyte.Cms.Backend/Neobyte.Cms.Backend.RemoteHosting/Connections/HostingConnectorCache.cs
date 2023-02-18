using MoreCSharp.Extensions.System.Collections.Generic;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using System.Collections.Concurrent;

namespace Neobyte.Cms.Backend.RemoteHosting.Connections;

internal class HostingConnectorCache {

	private readonly ConcurrentDictionary<HostingConnectionId, IRemoteHostingConnector> _cache = new();

	public bool AddConnector (HostingConnectionId id, IRemoteHostingConnector connector) {
		return _cache.TryAdd(id, connector);
	}

	public bool TryGetConnector (HostingConnectionId id, out IRemoteHostingConnector? connector) {
		return _cache.TryGetValue(id, out connector);
	}
	
	public IEnumerable<(HostingConnectionId id, IRemoteHostingConnector connector)> GetConnectors () {
		return _cache.Select(kvp => (kvp.Key, kvp.Value));
	}

	public bool RemoveConnector (HostingConnectionId id) {
		bool success = _cache.TryRemove(id, out var connector);
		connector?.Dispose();
		return success;
	}

	~HostingConnectorCache () {
		Clear();
	}

	public void Clear () {
		_cache.Keys.ForEach(c => RemoveConnector(c));
	}

}