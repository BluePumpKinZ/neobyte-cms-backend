namespace Neobyte.Cms.Backend.Core.Configuration; 

public class PuppeteerOptions {

	public string ApiScheme { get; set; } = string.Empty;
	public string ApiHost { get; set; } = string.Empty;
	public int ApiPort { get; set; } = 0;
	public bool RunInstallation { get; set; } = false;
	public bool RunOnStartup { get; set; } = false;

}