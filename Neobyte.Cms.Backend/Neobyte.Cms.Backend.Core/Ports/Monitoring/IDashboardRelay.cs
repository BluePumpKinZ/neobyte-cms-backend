using Microsoft.AspNetCore.Http;

namespace Neobyte.Cms.Backend.Core.Ports.Monitoring; 

public interface IDashboardRelay {

	public Task<HttpResponse> ForwardHttpRequest (HttpRequest request);

}