using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Identity; 

internal class IdentityUser : Microsoft.AspNetCore.Identity.IdentityUser {

	public Account? Account { get; set; }

}