using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountChangeDetailsOwnerRequestModel : AccountChangeDetailsRequestModel {

	public AccountId AccountId { get; set; }
	[Required]
	public bool Enabled { get; set; }
	[Required]
	public string[] Roles { get; set; } = null!;

}