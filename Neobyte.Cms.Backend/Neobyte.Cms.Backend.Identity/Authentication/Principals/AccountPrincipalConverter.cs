using Microsoft.IdentityModel.Tokens;
using Neobyte.Cms.Backend.Domain.Accounts;
using System;
using System.Linq;

namespace Neobyte.Cms.Backend.Identity.Authentication.Principals;

internal class AccountPrincipalConverter : IPrincipalConverter<Account, AccountId, AccountPrincipal> {

	public AccountPrincipal FromUser (Account user) {
		return new AccountPrincipal {
			Id = user.Id,
			Roles = user.AccountRoles!.Select(ar => ar.Role!.Name).ToArray()
		};
	}

	public (bool valid, AccountPrincipal? principal) FromTokenValidationResult (TokenValidationResult tokenValidationResult) {
		if (!tokenValidationResult.IsValid)
			return (false, null);


		var userId = (string)tokenValidationResult.Claims.First(c => c.Key == "id").Value;
		var roles = ((string)tokenValidationResult.Claims.First(c => c.Key == "roles").Value).Split(':');

		return (true, new AccountPrincipal {
			Id = new AccountId(Guid.Parse(userId)),
			Roles = roles
		});
	}

}