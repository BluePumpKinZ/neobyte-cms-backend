namespace Neobyte.Cms.Backend.Api.Filters.Authorization.Extensions; 

internal static class RouteGroupBuilderExtensions {

	public static RouteGroupBuilder FilterEnabledAccounts (this RouteGroupBuilder builder) {
		return builder.AddEndpointFilter<EnabledAccountFilter>();
	}

}