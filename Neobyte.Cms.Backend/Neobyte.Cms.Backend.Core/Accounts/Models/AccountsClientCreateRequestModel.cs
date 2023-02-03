namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountsClientCreateRequestModel {

	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;

}