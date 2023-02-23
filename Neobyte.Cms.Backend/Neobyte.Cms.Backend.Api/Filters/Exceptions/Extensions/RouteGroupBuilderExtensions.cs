namespace Neobyte.Cms.Backend.Api.Filters.Exceptions.Extensions;

public static class RouteGroupBuilderExtensions {

	public static RouteGroupBuilder HandleApplicationExceptions (this RouteGroupBuilder builder) {
		return builder.AddEndpointFilter<ExceptionEndpointFilter>();
	}

}