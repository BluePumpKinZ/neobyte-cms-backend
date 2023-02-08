namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountChangePasswordResponseModel {

	public bool Success { get; set; }
	public string[]? Errors { get; set; }

	public AccountChangePasswordResponseModel (bool success, string[]? errors) {
		Success = success;
		Errors = errors;
	}

}