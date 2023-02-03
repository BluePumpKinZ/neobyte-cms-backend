namespace Neobyte.Cms.Backend.Core.Configuration; 

public class DefaultAccountOptions {

	public bool AddOnAccountsEmpty { get; set; } = false;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;

}