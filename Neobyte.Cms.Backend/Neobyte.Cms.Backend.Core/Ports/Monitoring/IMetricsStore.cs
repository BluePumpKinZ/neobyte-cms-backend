using Prometheus;

namespace Neobyte.Cms.Backend.Core.Ports.Monitoring;

public interface IMetricsStore {

	public Gauge ActiveRemoteHostingConnections { get; }

}