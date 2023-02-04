using Microsoft.Extensions.DependencyInjection;

namespace Neobyte.Cms.Backend.Api.Endpoints.Loader;

internal class ApiEndpointLoader {

	public void LoadEndpoints (WebApplication app) {

		foreach (var endpoint in app.Services.GetServices<IApiEndpoints>()) {
			endpoint.RegisterApis(app.MapGroup(endpoint.Path)
				.WithTags(endpoint.GroupName));
		}
	}

}