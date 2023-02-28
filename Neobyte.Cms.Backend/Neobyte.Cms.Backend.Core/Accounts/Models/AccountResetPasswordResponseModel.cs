namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountResetPasswordResponseModel {
	
	public bool Success { get; set; }
	public string[]? Errors { get; set; }

	public AccountResetPasswordResponseModel (bool success) {
		Success = success;
	}
}