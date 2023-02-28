using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.RemoteHosting.Connections; 

class HostingConnectorCacheEntry {

	public HostingConnection Connection { get; }
	public IRemoteHostingConnector? Connector { get; }

	public HostingConnectorCacheEntry (HostingConnection connection, IRemoteHostingConnector? connector) {
		Connection = connection;
		Connector = connector;
	}

	public override bool Equals (object? obj) {
		return obj is HostingConnectorCacheEntry entry &&
			   entry.Connection.Equals(Connection);
	}

	public override int GetHashCode () {
		return Connection.GetHashCode();
	}

}