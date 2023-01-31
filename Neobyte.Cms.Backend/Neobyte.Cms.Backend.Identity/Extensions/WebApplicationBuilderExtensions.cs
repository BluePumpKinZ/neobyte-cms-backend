using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Adapters;
using Neobyte.Cms.Backend.Identity.Authentication;
using Neobyte.Cms.Backend.Identity.Authentication.Passwords;
using Neobyte.Cms.Backend.Identity.Authentication.Principals;
using Neobyte.Cms.Backend.Identity.Configuration;
using System.IdentityModel.Tokens.Jwt;
using PasswordHasher = Microsoft.AspNetCore.Identity.PasswordHasher<Neobyte.Cms.Backend.Domain.Accounts.Account>;

namespace Neobyte.Cms.Backend.Identity.Extensions;

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddIdentity (this WebApplicationBuilder builder) {

		builder.Services.AddScoped<IIdentityAuthenticationProvider, IdentityAuthenticationProvider>();
		builder.Services.AddScoped<IIdentityAuthorizationProvider, IdentityAuthorizationProvider>();
		builder.Services.AddScoped<IIdentityRoleProvider, IdentityRoleProvider>();

		builder.Services.AddScoped<AuthenticationManager>();

		builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection("Identity"));
		builder.Services.AddSingleton<JwtSecurityTokenHandler>();
		builder.Services.AddSingleton<JwtManager<Account, AccountId, AccountPrincipal>>();
		builder.Services.AddSingleton<IPrincipalConverter<Account, AccountId, AccountPrincipal>, AccountPrincipalConverter>();
		builder.Services.AddSingleton<PasswordHasher>();
		builder.Services.AddSingleton<PasswordValidator>();

		builder.Services.AddSingleton<IPasswordRule, RequireDigitPasswordRule>();
		builder.Services.AddSingleton<IPasswordRule, RequireLengthPasswordRule>();
		builder.Services.AddSingleton<IPasswordRule, RequireLowercasePasswordRule>();
		builder.Services.AddSingleton<IPasswordRule, RequireNonAlphaNumericPasswordRule>();
		builder.Services.AddSingleton<IPasswordRule, RequireUppercasePasswordRule>();

		return builder;
	}

}