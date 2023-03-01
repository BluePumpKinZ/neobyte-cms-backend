namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountRequestResetPasswordResponseModel {
	public bool Success { get; set; }
	public string[]? Errors { get; set; }
	
	public AccountRequestResetPasswordResponseModel (bool success) {
		Success = success;
	}
}