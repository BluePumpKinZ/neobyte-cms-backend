using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Core.Identity.Models.Authentication; 

public class IdentityAuthenticateResponseModel {

	public bool IsAuthenticated { get; set; }
	public AccountId? AccountId { get; set; }
	public string[]? Roles { get; set; }

	private IdentityAuthenticateResponseModel (AccountId? accountId, string[]? roles, bool isAuthenticated) {
		AccountId = accountId;
		Roles = roles;
		IsAuthenticated = isAuthenticated;
	}

	public static IdentityAuthenticateResponseModel Authenticated (AccountId accountId, string[] roles, bool isAuthenticated) {
		return new IdentityAuthenticateResponseModel(accountId, roles, isAuthenticated);
	}

	public static IdentityAuthenticateResponseModel Unauthenticated () {
		return new IdentityAuthenticateResponseModel(null, null, false);
	}

}