namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountChangeDetailsRequestModel {

	[Required]
	public string FirstName { get; set; } = null!;
	[Required]
	public string LastName { get; set; } = null!;
}