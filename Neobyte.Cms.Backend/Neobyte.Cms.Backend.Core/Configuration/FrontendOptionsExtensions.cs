namespace Neobyte.Cms.Backend.Core.Configuration; 

public static class FrontendOptionsExtensions {
	
	public static string GetUrl(this FrontendOptions options) {
		return $"{options.Scheme}://{options.Host}:{options.Port}";
	}
}