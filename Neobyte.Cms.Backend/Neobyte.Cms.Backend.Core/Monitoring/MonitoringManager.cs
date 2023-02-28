using Microsoft.AspNetCore.Http;
using Neobyte.Cms.Backend.Core.Ports.Monitoring;

namespace Neobyte.Cms.Backend.Core.Monitoring; 

public class MonitoringManager {

	private readonly IDashboardRelay _dashboardRelay;

	public MonitoringManager (IDashboardRelay dashboardRelay) {
		_dashboardRelay = dashboardRelay;
	}

	public async Task<HttpResponse> RelayDashboardRequest (HttpRequest request) {
		return await _dashboardRelay.ForwardHttpRequest(request);
	}

}