using Microsoft.AspNetCore.Identity;

namespace Neobyte.Cms.Backend.Persistence.Entities.Accounts;

public class IdentityAccountEntity : IdentityUser<Guid> {

	public IdentityAccountEntity () {
		base.Id = Guid.NewGuid();
	}

	public AccountEntity? Account { get; set; }

}