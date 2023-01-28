namespace Neobyte.Cms.Backend.Core.Identity.Models.Authentication;

public class IdentityLoginRequestModel {

	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public bool RememberMe { get; set; }

}