using Neobyte.Cms.Backend.Core.Ports.Monitoring;
using Prometheus;

namespace Neobyte.Cms.Backend.Monitoring.Adapters; 

internal class MetricsStore : IMetricsStore {

	public Gauge ActiveRemoteHostingConnections { get; }
		= Metrics.CreateGauge("cms_remotehosting_active_connections", "Number active and open connection to external hosting providers");

}