using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountRequestResetPasswordRequestModel {
	[Required]
	public string Email { get; set; } = null!;
	public string Scheme { get; set; } = null!;
	public string Host { get; set; } = null!;
}