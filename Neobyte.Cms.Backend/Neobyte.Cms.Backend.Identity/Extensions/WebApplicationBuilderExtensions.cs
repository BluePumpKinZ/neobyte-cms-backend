using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Neobyte.Cms.Backend.Identity.Extensions; 

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddIdentity (this WebApplicationBuilder builder) {

		builder.Services.AddIdentity<IdentityUser, IdentityRole>();

		return builder;
	}

}