namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountResetPasswordRequestModel {
	[Required]
	public string Password { get; set; } = null!;
	[Required]
	[Compare(nameof(Password))]
	public string ConfirmPassword { get; set; } = null!;
	[Required]
	public string Token { get; set; } = null!;
	[Required]
	public string Email { get; set; } = null!;
	
}