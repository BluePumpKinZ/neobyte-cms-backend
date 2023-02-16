namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountChangeDetailsRequestModel {

	[Required]
	public string Email { get; set; } = null!;
	[Required]
	public string Username { get; set; } = null!;
	[Required]
	public string Bio { get; set; } = null!;
}