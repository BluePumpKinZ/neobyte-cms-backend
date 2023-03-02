using System;
using System.Collections.Generic;
using Yarp.ReverseProxy.Configuration;

namespace Neobyte.Cms.Backend.Monitoring.Configuration;

public static class MonitoringOptionsExtensions {

	public static RouteConfig[] GetRoutes (this MonitoringOptions options) {

		return new[] {
				new RouteConfig {
					RouteId = "dashboard-routes",
					ClusterId = options.Dashboard.Cluster,
					Match = new RouteMatch {
						Path = "/api/v1/monitoring/dashboard/{**catch-all}"
					}
				},
				new RouteConfig {
					RouteId = "frontend-tracing-routes",
					ClusterId = options.Frontend.Cluster,
					Match = new RouteMatch {
						Path = "/api/traces/{**catch-all}"
					}
				}
			};
	}

	public static ClusterConfig[] GetClusters (this MonitoringOptions options) {

		return new[] {
			new ClusterConfig {
				ClusterId = options.Dashboard.Cluster,
				Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) {
					{
						"dashboard-destination", new DestinationConfig
						{ Address = $"http://{options.Dashboard.Host}:{options.Dashboard.Port}/" }
					}
				}
			},
			new ClusterConfig {
				ClusterId = options.Frontend.Cluster,
				Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) {
					{
						"frontend-tracing-destination", new DestinationConfig
						{ Address = $"http://{options.Frontend.Host}:{options.Frontend.Port}/" }
					}
				}
			}
		};
	}

}