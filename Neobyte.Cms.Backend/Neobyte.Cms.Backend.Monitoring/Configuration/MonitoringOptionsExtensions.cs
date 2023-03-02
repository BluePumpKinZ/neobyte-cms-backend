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
						Path = "/api/v1/monitoring/dashboard/{**catch-all}"
					}
				},
				new RouteConfig {
					RouteId = "zipkin-routes",
					ClusterId = "zipkin-clusters",
					Match = new RouteMatch {
						Path = "/api/traces/{**catch-all}"
					}
				}
			};
	}

	public static ClusterConfig[] GetClusters (this MonitoringDashboardOptions options) {

		return new[] {
			new ClusterConfig {
				ClusterId = options.Cluster,
				Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) {
					{
						"dashboard-destination", new DestinationConfig
						{ Address = $"http://{options.Host}:{options.Port}/" }
					}
				}
			},
			new ClusterConfig {
				ClusterId = "zipkin-clusters",
				Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) {
					{
						"zipkin-destination", new DestinationConfig
						{ Address = $"http://localhost:9411/" }
					} 
				}
			}
		};
	}

}