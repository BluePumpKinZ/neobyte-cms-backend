using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.RemoteHosting.Adapters; 

public class RemoteHostingProvider : IRemoteHostingProvider {

	private readonly IEnumerable<IRemoteHostingConnector> _hostingConnectors;

	public RemoteHostingProvider (IEnumerable<IRemoteHostingConnector> hostingConnectors) {
		_hostingConnectors = hostingConnectors;
	}

	public IRemoteHostingConnector CreateConnection (HostingConnection connection) {
		var hostingConnector = _hostingConnectors.Single(hc => hc.CanConnect(connection));
		hostingConnector.Configure(connection);
		return hostingConnector;
	}

}