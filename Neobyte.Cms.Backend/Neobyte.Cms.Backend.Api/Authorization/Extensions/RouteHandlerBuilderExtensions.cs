namespace Neobyte.Cms.Backend.Api.Authorization.Extensions;

public static class RouteHandlerBuilderExtensions {

	public static RouteHandlerBuilder Authorize (this RouteHandlerBuilder builder, UserPolicy policy) {
		return builder.RequireAuthorization(policy.Name);
	}

}