using Microsoft.AspNetCore.Http;
using Neobyte.Cms.Backend.Core.Identity;

namespace Neobyte.Cms.Backend.Api.Endpoints.Authorization.Extensions;

internal static class RouteHandlerBuilderExtensions {

	public static RouteHandlerBuilder Authorize (this RouteHandlerBuilder builder, Privileges privileges) {

		builder.AddEndpointFilter((context, next) => {
			context.HttpContext.Items.Add(AuthorizationFilter.MetadataKey, new AuthorizationMetadata(privileges));
			return next(context);
		});
		builder.AddEndpointFilter<AuthorizationFilter>();
		return builder;
	}

}