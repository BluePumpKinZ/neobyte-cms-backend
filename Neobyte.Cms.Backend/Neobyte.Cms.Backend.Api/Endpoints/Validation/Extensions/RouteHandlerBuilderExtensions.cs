using Microsoft.AspNetCore.Http;

namespace Neobyte.Cms.Backend.Api.Endpoints.Validation.Extensions; 

public static class RouteHandlerBuilderExtensions {

	public static RouteHandlerBuilder ValidateBody<T> (this RouteHandlerBuilder builder)
		where T : class {
		return builder.AddEndpointFilter<EndpointValidationFilter<T>>();
	}

}