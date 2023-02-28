namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountsGeneratePasswordResetTokenResponseModel {
	public bool Success { get; set; }
	public string? Token { get; set; }
	public string[]? Errors { get; set; }
	
	public AccountsGeneratePasswordResetTokenResponseModel(bool success) {
		Success = success;
	}
}