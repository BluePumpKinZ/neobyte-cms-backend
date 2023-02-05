using Neobyte.Cms.Backend.Domain.Accounts;
using System;
using System.Collections.Generic;

namespace Neobyte.Cms.Backend.Api.Authorization; 

public class Principal {

	public AccountId AccountId { get; set; }
	public Guid IdentityAccountId { get; set; }
	public IEnumerable<string> Roles { get; set; }

	public Principal (AccountId accountId, Guid identityAccountId, IEnumerable<string> roles) {
		AccountId = accountId;
		IdentityAccountId = identityAccountId;
		Roles = roles;
	}

}