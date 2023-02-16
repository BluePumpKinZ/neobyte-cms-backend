using Microsoft.AspNetCore.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Persistence.Entities.Accounts;

public class IdentityAccountEntity : IdentityUser<Guid> {

	public IdentityAccountEntity () {
		base.Id = Guid.NewGuid();
	}

	public AccountEntity? Account { get; set; }

	internal Account ToDomain (string[] roles) {
		return new Account(Account!.Id, Email!, Account!.Username, Account!.Bio, Account!.Enabled, Account!.CreationDate, roles);
	}

}