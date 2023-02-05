using Microsoft.AspNetCore.Identity;

namespace Neobyte.Cms.Backend.Domain.Accounts;

public class IdentityAccount : IdentityUser<Guid> {

	public IdentityAccount () {
		base.Id = Guid.NewGuid();
	}

	public Account? Account { get; set; }

}