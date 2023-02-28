namespace Neobyte.Cms.Backend.RemoteHosting.Configuration; 

public class RemoteHostingOptions {

	public const string Section = "RemoteHosting";
	public int ConnectionTimeout { get; set; } = 60;

}