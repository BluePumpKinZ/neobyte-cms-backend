namespace Neobyte.Cms.Backend.Core.Identity.Models.Authentication; 

public class IdentityLoginResponseModel {

	public bool Authenticated { get; }
	public string? Token { get; }
	public long? Expires { get; }

	public IdentityLoginResponseModel (bool authenticated, string? token, long? expires) {
		Authenticated = authenticated;
		Token = token;
		Expires = expires;
	}

}