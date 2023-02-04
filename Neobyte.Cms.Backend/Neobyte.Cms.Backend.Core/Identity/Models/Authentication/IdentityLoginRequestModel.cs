namespace Neobyte.Cms.Backend.Core.Identity.Models.Authentication; 

public class IdentityLoginRequestModel {

	[Required]
	[EmailAddress]
	public string Email { get; set; } = string.Empty;
	[Required]
	public string Password { get; set; } = string.Empty;
	[Required]
	public bool RememberMe { get; set; }

}