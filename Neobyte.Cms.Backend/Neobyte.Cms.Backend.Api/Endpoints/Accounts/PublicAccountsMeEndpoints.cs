using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Accounts.Models;

namespace Neobyte.Cms.Backend.Api.Endpoints.Accounts;

public class PublicAccountsMeEndpoints : IApiEndpoints {
	public string GroupName => "PublicAccount";
	public string Path => "/api/v1/accounts/me/public";
	public bool Authorized => false;

	public void RegisterApis (RouteGroupBuilder routes) {
		routes.MapPut("reset-password", async (
				[FromServices] AccountManager manager,
				[FromBody] AccountResetPasswordRequestModel request) => {
				var response = await manager.ResetPasswordAsync(request);
				return response.Success
					? Results.Ok(new {Message = "Password reset", Valid = true})
					: Results.BadRequest(new {response.Errors, Valid = false});
			})
			.ValidateBody<AccountResetPasswordRequestModel>();

	}
}