﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Core.Accounts;
using Neobyte.Cms.Backend.Core.Configuration;
using Neobyte.Cms.Backend.Core.Websites.Managers;

namespace Neobyte.Cms.Backend.Core.Extensions;

public static class WebApplicationExtensions {

	public static WebApplication UseCore (this WebApplication app) {

		var coreOptions = app.Services.GetRequiredService<IOptions<CoreOptions>>().Value;

		using var scope = app.Services.CreateScope();
		var defaultAccountCreator = scope.ServiceProvider.GetRequiredService<DefaultAccountCreator>();
		defaultAccountCreator.CreateDefaultAccount().Wait();

		// Make sure a browser instance is created, so that it is ready when the first request comes in.
		// When first ran, this will download a chromium installation, which can take a while.
		if (coreOptions.Puppeteer.RunOnStartup) {
			var browserManager = scope.ServiceProvider.GetRequiredService<BrowserManager>();
			browserManager.ExecuteOnBrowserAsync(_ => Task.CompletedTask).Wait();
		}

		return app;
	}

}