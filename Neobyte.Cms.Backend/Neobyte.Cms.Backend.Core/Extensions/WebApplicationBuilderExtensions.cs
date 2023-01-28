using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Identity.Managers;

namespace Neobyte.Cms.Backend.Core.Extensions;

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddCore (this WebApplicationBuilder builder) {

		builder.Services.AddScoped<IdentityAuthenticationManager>();
		builder.Services.AddScoped<IdentityAuthorizationManager>();
		builder.Services.AddScoped<IdentityRoleManager>();

		return builder;
	}

}