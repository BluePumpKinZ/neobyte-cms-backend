namespace Neobyte.Cms.Backend.Core.Configuration; 

public class CoreOptions {

	public const string Section = "Core";
	public DefaultAccountOptions DefaultAccount { get; set; } = new DefaultAccountOptions();
	public PuppeteerOptions Puppeteer { get; set; } = new PuppeteerOptions();
	public FrontendOptions Frontend { get; set; } = new FrontendOptions();

}