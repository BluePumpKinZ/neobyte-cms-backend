using Neobyte.Cms.Backend.Core.Identity;

namespace Neobyte.Cms.Backend.Api.Authorization.Extensions;

public static class RouteHanlderBuilderExtensions {

	public static RouteHandlerBuilder Authorize (this RouteHandlerBuilder builder, UserPolicy policy) {
		return builder.RequireAuthorization(policy.Name);
	}

}