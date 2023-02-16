namespace Neobyte.Cms.Backend.Core.Configuration; 

public class CoreOptions {

	public const string Section = "Core";
	public DefaultAccountOptions DefaultAccount { get; set; } = new DefaultAccountOptions();

}