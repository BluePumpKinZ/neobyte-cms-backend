using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Identity.Adapters;

namespace Neobyte.Cms.Backend.Identity.Extensions;

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddIdentity (this WebApplicationBuilder builder) {

		builder.Services.AddScoped<IIdentityAuthenticationProvider, IdentityAuthenticationProvider>();
		builder.Services.AddScoped<IIdentityAuthorizationProvider, IdentityAuthorizationProvider>();
		builder.Services.AddScoped<IIdentityRoleProvider, IdentityRoleProvider>();

		builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection("Identity"));

		return builder;
	}

}