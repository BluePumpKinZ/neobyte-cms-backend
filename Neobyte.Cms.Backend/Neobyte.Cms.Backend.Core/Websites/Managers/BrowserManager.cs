using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Core.Configuration;
using Neobyte.Cms.Backend.Utils;
using PuppeteerSharp;
using System;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class BrowserManager {

	private readonly AsyncUtils _asyncUtils = new();
	private readonly ILogger<BrowserManager> _logger;
	private readonly PuppeteerOptions _puppeteerOptions;
	private IBrowser? _browser;

	public BrowserManager (ILogger<BrowserManager> logger, IOptions<CoreOptions> options) {
		_logger = logger;
		_puppeteerOptions = options.Value.Puppeteer;
	}

	private async Task<IBrowser> GetBrowserInstanceAsync () {
		_browser ??= await CreateBrowserAsync();
		return _browser;
	}

	private async Task<IBrowser> CreateBrowserAsync () {
		_logger.LogInformation("Getting browser instance");

		if (_puppeteerOptions.RunInstallation) {
			using var fetcher = new BrowserFetcher();
			_ = await fetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
		}

		var browser = await Puppeteer.LaunchAsync(new LaunchOptions {
			Headless = true,
			DefaultViewport = new ViewPortOptions {
				Width = 1920, Height = 1080
			},
			Args = new string[] {
				"--no-sandbox",
				"--disable-setuid-sandbox"
			}
		});
		_logger.LogInformation("Got browser instance");
		return browser;
	}

	public async Task<T> ExecuteOnBrowserAsync<T> (Func<IBrowser, Task<T>> function) {

		return await _asyncUtils.LockAsync(async () => {
			var browser = await GetBrowserInstanceAsync();
			return await function.Invoke(browser);
		});
	}

	public async Task ExecuteOnBrowserAsync (Func<IBrowser, Task> function) {

		await _asyncUtils.LockAsync(async () => {
			var browser = await GetBrowserInstanceAsync();
			await function.Invoke(browser);
		});
	}


}