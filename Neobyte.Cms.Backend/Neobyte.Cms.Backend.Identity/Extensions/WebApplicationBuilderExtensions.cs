using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Policies;
using Neobyte.Cms.Backend.Persistence.EF;

namespace Neobyte.Cms.Backend.Identity.Extensions;

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddIdentity (this WebApplicationBuilder builder, Action<IdentityOptions> options) {

		builder.Services.AddIdentity<AccountIdentityUser, IdentityRole>()
			.AddEntityFrameworkStores<EFDbContext>()
			.AddDefaultTokenProviders();

		builder.Services.Configure(options);

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