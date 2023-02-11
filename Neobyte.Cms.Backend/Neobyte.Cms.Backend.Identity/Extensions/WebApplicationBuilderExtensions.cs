using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Identity.Adapters;
using Neobyte.Cms.Backend.Identity.Configuration;
using Neobyte.Cms.Backend.Identity.Initializers;
using Neobyte.Cms.Backend.Persistence.EF;
using Neobyte.Cms.Backend.Persistence.Entities.Accounts;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Neobyte.Cms.Backend.Identity.Extensions;

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddIdentity (this WebApplicationBuilder builder) {

		builder.Services.AddScoped<IIdentityAuthenticationProvider, IdentityAuthenticationProvider>();
		builder.Services.AddScoped<IIdentityAuthorizationProvider, IdentityAuthorizationProvider>();

		builder.Services.AddScoped<RoleInitializer>();

		builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection("Identity"));

		builder.Services.AddIdentity<IdentityAccountEntity, IdentityRole<Guid>>()
			.AddEntityFrameworkStores<EFDbContext>()
			.AddDefaultTokenProviders();

		var jwtOptions = new JwtOptions();
		builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.Section));
		builder.Configuration.GetSection(JwtOptions.Section).Bind(jwtOptions);

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
		builder.Services.AddSingleton(key);
		builder.Services.AddSingleton(credentials);
		builder.Services.AddSingleton<JwtSecurityTokenHandler>();

		builder.Services.AddAuthentication(options => {
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options => {
			options.TokenValidationParameters = new TokenValidationParameters {
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = jwtOptions.Issuer,
				ValidAudience = jwtOptions.Audience,
				IssuerSigningKey = key,
				ClockSkew = TimeSpan.Zero
			};
		});

		builder.Services.AddAuthorization(options => {
			foreach (UserPolicy userPolicy in UserPolicy.All) {
				options.AddPolicy(userPolicy.Name, policy => {
					policy.RequireAuthenticatedUser();
					policy.RequireRole(userPolicy.Roles.Select(r => r.RoleName));
				});
			}
		});

		return builder;
	}

}