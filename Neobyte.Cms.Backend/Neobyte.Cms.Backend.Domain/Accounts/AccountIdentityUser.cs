namespace Neobyte.Cms.Backend.Domain.Accounts; 

public class AccountIdentityUser : Microsoft.AspNetCore.Identity.IdentityUser {

	public Account? Account { get; set; }

	public AccountIdentityUser () {}

}