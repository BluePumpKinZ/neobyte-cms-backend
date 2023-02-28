namespace Neobyte.Cms.Backend.Api.Filters.Authorization.Extensions;

internal static class RouteHandlerBuilderExtensions {

	public static RouteHandlerBuilder Authorize (this RouteHandlerBuilder builder, UserPolicy policy) {
		return builder.RequireAuthorization(policy.Name)
			.Produces(StatusCodes.Status401Unauthorized);
	}

}