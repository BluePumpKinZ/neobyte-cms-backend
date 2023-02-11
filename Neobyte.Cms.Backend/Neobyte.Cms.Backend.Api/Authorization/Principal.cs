using Neobyte.Cms.Backend.Domain.Accounts;
using System.Collections.Generic;

namespace Neobyte.Cms.Backend.Api.Authorization; 

public class Principal {

	public AccountId AccountId { get; set; }
	public IEnumerable<string> Roles { get; set; }

	public Principal (AccountId accountId, IEnumerable<string> roles) {
		AccountId = accountId;
		Roles = roles;
	}

}