namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountsCreateRequestModel {

	public string Username { get; set; } = string.Empty;
	public string Bio { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;

}