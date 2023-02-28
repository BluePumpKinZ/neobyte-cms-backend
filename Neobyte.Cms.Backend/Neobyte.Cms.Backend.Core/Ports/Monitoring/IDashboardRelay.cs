using System.Net.Http;

namespace Neobyte.Cms.Backend.Core.Ports.Monitoring;

public interface IDashboardRelay {

	public Task<HttpResponseMessage> ForwardHttpRequest (HttpRequestMessage request);

}