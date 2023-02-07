namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountChangePasswordRequestModel {

	[Required]
	public string OldPassword { get; set; } = null!;
	[Required]
	[Compare(nameof(ConfirmPassword))]
	public string NewPassword { get; set; } = null!;
	[Required]
	public string ConfirmPassword { get; set; } = null!;

}