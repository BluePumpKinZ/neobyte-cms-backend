using Neobyte.Cms.Backend.Utils;
using PuppeteerSharp;
using System;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class BrowserManager {

	private readonly AsyncUtils _asyncUtils = new();
	private IBrowser? _browser;

	private async Task<IBrowser> GetBrowserInstanceAsync () {
		using var fetcher = new BrowserFetcher();
		_ = await fetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
		_browser ??= await CreateBrowserAsync();
		return _browser;
	}

	private static async Task<IBrowser> CreateBrowserAsync () {
		return await Puppeteer.LaunchAsync(new LaunchOptions {
			// Headless = true,
			DefaultViewport = new ViewPortOptions {
				Width = 1920, Height = 1080,
			}
		});
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