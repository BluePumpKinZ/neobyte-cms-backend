using System.ComponentModel.DataAnnotations;

namespace Neobyte.Cms.Backend.Core.Identity.Models.Authentication; 

public class IdentityRegisterRequestModel {

	[Required]
	public string Email { get; set; } = string.Empty;
	[Required]
	[Compare(nameof(ConfirmPassword))]
	public string Password { get; set; } = string.Empty;
	[Required]
	public string ConfirmPassword { get; set; } = string.Empty;
	[Required]
	public string FirstName { get; set; } = string.Empty;
	[Required]
	public string LastName { get; set; } = string.Empty;

}