using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Accounts;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Configuration;
using Neobyte.Cms.Backend.Core.Identity.Managers;
using Neobyte.Cms.Backend.Core.Mailing.Managers;
using Neobyte.Cms.Backend.Core.Websites.Managers;
using Neobyte.Cms.Backend.Core.Websites.Transformers;

namespace Neobyte.Cms.Backend.Core.Extensions;

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddCore (this WebApplicationBuilder builder) {

		builder.Services.Configure<CoreOptions>(builder.Configuration.GetSection(CoreOptions.Section));
		builder.Services.Configure<HtmlTransformerOptions>(builder.Configuration.GetSection(HtmlTransformerOptions.Section));

		builder.Services.AddScoped<AccountListManager>();
		builder.Services.AddScoped<AccountManager>();
		builder.Services.AddScoped<DefaultAccountCreator>();

		builder.Services.AddScoped<IdentityAuthenticationManager>();

		builder.Services.AddScoped<MailingManager>();

		builder.Services.AddSingleton<HtmlTransformer>();
		builder.Services.AddScoped<WebsiteManager>();
		builder.Services.AddScoped<WebsitePageManager>();
		builder.Services.AddScoped<WebsiteSnippetManager>();
		builder.Services.AddScoped<WebsiteFileManager>();

		return builder;
	}

}