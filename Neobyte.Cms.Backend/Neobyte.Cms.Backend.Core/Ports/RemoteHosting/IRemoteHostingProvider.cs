using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Core.Ports.RemoteHosting; 

public interface IRemoteHostingProvider {

	public IRemoteHostingConnector GetConnector (HostingConnection connection);

}