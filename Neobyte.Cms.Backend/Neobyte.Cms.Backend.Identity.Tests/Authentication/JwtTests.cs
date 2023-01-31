using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.Extensions.Options;
using MoreCSharp.Extensions.System.Collections.Generic;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Authentication;
using Neobyte.Cms.Backend.Identity.Authentication.Principals;
using Neobyte.Cms.Backend.Identity.Configuration;
using NUnit.Framework;

namespace Neobyte.Cms.Backend.Identity.Tests.Authentication; 

public class JwtTests {

	private readonly JwtManager<Account, AccountId, AccountPrincipal> _jwtManager;

	public JwtTests () {
		_jwtManager = new JwtManager<Account, AccountId, AccountPrincipal> (
			new JwtSecurityTokenHandler(),
			Options.Create (new JwtOptions {
				Issuer = "Neobyte CMS",
				Audience = "Neobyte CMS",
				Secret = "JisI1YLXRLVipHjnK"
			}),
			new AccountPrincipalConverter());
		
	}

	[Test]
	public void JwtPrincipalEncodingAndDecodingWorks () {

		var id = AccountId.New();
		var roles = new string[] { "Owner", "Client83" };

		var accountPrincipal = new AccountPrincipal {
			Id = id,
			Roles = roles
		};

		var token = _jwtManager.GenerateToken(accountPrincipal, 86_400_000);
		var result = _jwtManager.ValidateTokenAsync(token).Result;

		Assert.IsTrue(result.valid);
		Assert.AreEqual(id, result.principal!.Id);
		Assert.AreEqual(roles.Length, result.principal.Roles.Length);
		roles.Zip(result.principal.Roles).ForEach((pair) => {
			Assert.AreEqual(pair.First, pair.Second);
		});

	}

}