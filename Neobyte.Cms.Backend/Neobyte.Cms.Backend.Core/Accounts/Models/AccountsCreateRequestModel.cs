namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountsCreateRequestModel {
	[Required]
	public string Username { get; set; } = string.Empty;
	[Required]
	public string Bio { get; set; } = string.Empty;
	[Required]
	public string Email { get; set; } = string.Empty;
	[Required]
	public string Role { get; set; } = string.Empty;
	public string Scheme { get; set; } = string.Empty;
	public string Host { get; set; } = string.Empty;
}