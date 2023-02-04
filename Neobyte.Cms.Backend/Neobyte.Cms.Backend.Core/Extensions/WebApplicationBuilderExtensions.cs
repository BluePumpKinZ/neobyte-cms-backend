using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Accounts;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Configuration;
using Neobyte.Cms.Backend.Core.Identity.Managers;
using Neobyte.Cms.Backend.Core.Mailing.Managers;

namespace Neobyte.Cms.Backend.Core.Extensions;

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddCore (this WebApplicationBuilder builder) {

		builder.Services.Configure<CoreOptions>(builder.Configuration.GetSection("Core"));

		builder.Services.AddScoped<AccountManager>();
		builder.Services.AddScoped<DefaultAccountCreator>();

		builder.Services.AddScoped<IdentityAuthenticationManager>();
		// builder.Services.AddScoped<IdentityAuthorizationManager>();
		
		builder.Services.AddScoped<MailingManager>();

		return builder;
	}

}