using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Core.Ports.Monitoring;
using Neobyte.Cms.Backend.Monitoring.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Monitoring.Adapters;

internal class DashboardRelay : IDashboardRelay {

	private readonly MonitoringDashboardOptions _options;

	public DashboardRelay (IOptions<MonitoringOptions> options) {
		_options = options.Value.Dashboard;
	}

	public async Task<HttpResponseMessage> ForwardHttpRequest (HttpRequestMessage request) {

		// change request host
		HttpClient httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri($"http://{_options.Host}:{_options.Port}");
		return await httpClient.SendAsync(request);

	}

}