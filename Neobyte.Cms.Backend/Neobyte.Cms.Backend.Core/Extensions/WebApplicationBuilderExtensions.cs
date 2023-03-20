using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Accounts;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Configuration;
using Neobyte.Cms.Backend.Core.Identity.Managers;
using Neobyte.Cms.Backend.Core.RemoteHosting.Managers;
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

		builder.Services.AddScoped<IdentityManager>();

		builder.Services.AddScoped<RemoteHostingManager>();
		builder.Services.AddScoped<HomeRemoteHostingManager>();
		builder.Services.AddScoped<PublicRemoteHostingManager>();
		builder.Services.AddScoped<UploadRemoteHostingManager>();

		builder.Services.AddSingleton<HtmlTransformer>();
		builder.Services.AddSingleton<IHtmlTransformer, BaseTagHtmlTransformer>();
		builder.Services.AddSingleton<IHtmlTransformer, CmsEditableHtmlTransformer>();
		builder.Services.AddSingleton<IHtmlTransformer, StylesHtmlTransformer>();
		builder.Services.AddSingleton<IHtmlTransformer, TinyMCEHtmlTransformer>();

		builder.Services.AddScoped<WebsiteAccountManager>();
		builder.Services.AddScoped<WebsiteFileManager>();
		builder.Services.AddScoped<WebsiteManager>();
		builder.Services.AddScoped<WebsitePageManager>();
		builder.Services.AddScoped<WebsiteSnippetManager>();
		builder.Services.AddScoped<WebsiteThumbnailManager>();

		builder.Services.AddSingleton<BrowserManager>();

		return builder;
	}

}