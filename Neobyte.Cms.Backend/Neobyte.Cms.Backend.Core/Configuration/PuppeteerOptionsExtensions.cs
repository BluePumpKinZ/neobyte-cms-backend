namespace Neobyte.Cms.Backend.Core.Configuration; 

public static class PuppeteerOptionsExtensions {

	public static string GetApiHost (this PuppeteerOptions options) {
		return $"{options.ApiScheme}://{options.ApiHost}:{options.ApiPort}/";
	}

}