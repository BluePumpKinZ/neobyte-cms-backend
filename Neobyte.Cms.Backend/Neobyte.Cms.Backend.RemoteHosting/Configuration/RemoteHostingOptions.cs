namespace Neobyte.Cms.Backend.RemoteHosting.Configuration; 

public class RemoteHostingOptions {

	public const string Section = "RemoteHosting";
	public int LastConnectionTimeout { get; set; } = 60;

}