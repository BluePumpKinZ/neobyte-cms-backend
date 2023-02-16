using Neobyte.Cms.Backend.Domain.Accounts;
using System.Collections.Generic;

namespace Neobyte.Cms.Backend.Api.Filters.Authorization;

internal class Principal {

	public AccountId AccountId { get; }
	public IEnumerable<string> Roles { get; }

	public Principal (AccountId accountId, IEnumerable<string> roles) {
		AccountId = accountId;
		Roles = roles;
	}

}