using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Accounts.Models;

namespace Neobyte.Cms.Backend.Api.Endpoints.Accounts; 

public class PublicAccountsMeEndpoints : IApiEndpoints{
	public string GroupName => "Account";
	public string Path => "/api/v1/accounts/me";
	public bool Authorized => false;
	
	public void RegisterApis (RouteGroupBuilder routes) {
		routes.MapPut("reset-password", async (
			[FromServices] AccountManager manager,
			[FromServices] Principal principal,
			[FromBody] AccountResetPasswordRequestModel request) => {
			var response = await manager.ResetPasswordAsync(request);
			return response.Success
				? Results.Ok(new { Message = "Password reset" })
				: Results.BadRequest(new { response.Errors });
		}).ValidateBody<AccountResetPasswordRequestModel>();
	}
}