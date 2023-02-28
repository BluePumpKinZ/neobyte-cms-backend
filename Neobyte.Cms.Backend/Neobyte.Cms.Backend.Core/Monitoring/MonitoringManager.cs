using Microsoft.AspNetCore.Http;
using Neobyte.Cms.Backend.Core.Ports.Monitoring;
using System;
using System.Net.Http;

namespace Neobyte.Cms.Backend.Core.Monitoring; 

public class MonitoringManager {

	private readonly IDashboardRelay _dashboardRelay;

	public MonitoringManager (IDashboardRelay dashboardRelay) {
		_dashboardRelay = dashboardRelay;
	}

	public async Task<HttpResponse> RelayDashboardRequest (HttpRequest request) {
		/*new HttpRequestMessage(request.Method)
		return await _dashboardRelay.ForwardHttpRequest(request);*/
		throw new NotImplementedException();
	}

}