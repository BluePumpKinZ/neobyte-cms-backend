using System.ComponentModel.DataAnnotations;

namespace Neobyte.Cms.Backend.Core.Identity.Models.Authentication;

public class IdentityLoginRequestModel {

	[Required]
	public string Email { get; set; } = string.Empty;
	[Required]
	public string Password { get; set; } = string.Empty;
	public bool RememberMe { get; set; }

}