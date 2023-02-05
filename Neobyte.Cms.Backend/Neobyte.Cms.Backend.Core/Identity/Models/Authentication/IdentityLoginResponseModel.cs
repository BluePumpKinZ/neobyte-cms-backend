namespace Neobyte.Cms.Backend.Core.Identity.Models.Authentication; 

public class IdentityLoginResponseModel {

	public bool Authenticated { get; }
	public string? Token { get; }

	public IdentityLoginResponseModel (bool authenticated, string? token) {
		Authenticated = authenticated;
		Token = token;
	}

}