namespace Neobyte.Cms.Backend.Core.Accounts.Models;

public class AccountRequestResetPasswordRequestModel {
	[Required]
	public string Email { get; set; } = null!;
}