using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Api.Filters.Exceptions.Extensions;

namespace Neobyte.Cms.Backend.Api.Endpoints.Loader;

internal class ApiEndpointLoader {

	public void LoadEndpoints (WebApplication app) {

		foreach (var endpoints in app.Services.GetServices<IApiEndpoints>()) {
			var group = app
				.MapGroup(endpoints.Path)
				.WithTags(endpoints.GroupName)
				.HandleApplicationExceptions();
			endpoints.RegisterApis(endpoints.Authorized
				? group.FilterEnabledAccounts()
				: group);
			
		}
	}

}