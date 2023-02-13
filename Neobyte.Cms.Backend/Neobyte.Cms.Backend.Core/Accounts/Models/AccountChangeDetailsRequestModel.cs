namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountChangeDetailsRequestModel {

	[Required]
	public string Email { get; set; }
	[Required]
	public string Username { get; set; }
	[Required]
	public string Bio { get; set; }
}