using Neobyte.Cms.Backend.Domain.Accounts;
using System;

namespace Neobyte.Cms.Backend.Identity.Authentication.Principals;

internal class AccountPrincipal : IPrincipal<AccountId> {

	public AccountId Id { get; set; }
	public string[] Roles { get; set; } = Array.Empty<string>();

}