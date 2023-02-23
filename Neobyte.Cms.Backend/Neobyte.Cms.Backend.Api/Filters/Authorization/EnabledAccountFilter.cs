using Neobyte.Cms.Backend.Core.Identity.Managers;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Api.Filters.Authorization;

internal class EnabledAccountFilter : IEndpointFilter {

	private readonly Principal _principal;
	private readonly IdentityManager _identityManager;

	public EnabledAccountFilter (Principal principal, IdentityManager identityManager) {
		_principal = principal;
		_identityManager = identityManager;
	}

	public async ValueTask<object?> InvokeAsync (EndpointFilterInvocationContext context, EndpointFilterDelegate next) {

		try {
			bool valid = await _identityManager.CanLoginAsync(_principal.AccountId);
			return valid ? await next(context) : Results.Forbid();
		} catch (Exception) {
			// account does not exist
			return Results.Unauthorized();
		}

	}

}