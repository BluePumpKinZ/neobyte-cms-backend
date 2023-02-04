using Microsoft.AspNetCore.Identity;

namespace Neobyte.Cms.Backend.Domain.Accounts;

public class IdentityAccount : IdentityUser<Guid> {

	public Account? Account { get; set; }

}