using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Adapters;
using Neobyte.Cms.Backend.Identity.Policies;
using Neobyte.Cms.Backend.Persistence.EF;

namespace Neobyte.Cms.Backend.Identity.Extensions;

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddIdentity (this WebApplicationBuilder builder) {

		builder.Services.AddScoped<IIdentityAuthenticationProvider, IdentityAuthenticationProvider>();
		builder.Services.AddScoped<IIdentityAuthorizationProvider, IdentityAuthorizationProvider>();
		builder.Services.AddScoped<IIdentityRoleProvider, IdentityRoleProvider>();

		builder.Services.AddIdentity<AccountIdentityUser, IdentityRole>()
			.AddEntityFrameworkStores<EFDbContext>()
			.AddDefaultTokenProviders();

		builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection("Identity"));

		builder.Services.ConfigureApplicationCookie(opt => {
			opt.LoginPath = "/login";
			opt.AccessDeniedPath = "/access-denied";
		});

		builder.Services.AddAuthorization(opt => {
			foreach (UserPolicy userPolicy in UserPolicyData.GetValues()) {
				opt.AddPolicy(userPolicy.GetPolicyName(), policy => {
					policy.RequireAuthenticatedUser();
					policy.RequireRole(userPolicy.GetPolicyRoles().Select(userRole => (string)userRole));
				});
			}
		});

		builder.Services.AddAuthentication();

		return builder;


	}

}