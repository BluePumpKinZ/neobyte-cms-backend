namespace Neobyte.Cms.Backend.Api.Filters.Validation.Extensions;

public static class RouteHandlerBuilderExtensions {

	public static RouteHandlerBuilder ValidateBody<T> (this RouteHandlerBuilder builder)
		where T : class {
		return builder.AddEndpointFilter<EndpointValidationFilter<T>>()
			.Produces(StatusCodes.Status400BadRequest);
	}

}