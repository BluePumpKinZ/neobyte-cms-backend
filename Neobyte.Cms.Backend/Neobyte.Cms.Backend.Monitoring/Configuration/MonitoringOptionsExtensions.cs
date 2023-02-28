using System;
using System.Collections.Generic;
using Yarp.ReverseProxy.Configuration;

namespace Neobyte.Cms.Backend.Monitoring.Configuration;

public static class MonitoringOptionsExtensions {

	public static RouteConfig[] GetRoutes (this MonitoringDashboardOptions options) {

		return new[] {
				new RouteConfig {
					RouteId = "dashboard-routes",
					ClusterId = options.Cluster,
					Match = new RouteMatch {
						Path = "{**catch-all}"
					}
				}
			};
	}

	public static ClusterConfig[] GetClusters (this MonitoringDashboardOptions options) {

		return new[] {
			new ClusterConfig {
				ClusterId = options.Cluster,
				Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) {
					{ "dashboard-destination", new DestinationConfig {
						Address = $"http://{options.Host}:{options.Port}/" }
					}
				}
			}
		};
	}

}