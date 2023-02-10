namespace Neobyte.Cms.Backend.RemoteHosting.Connectors; 

public class FtpConnectorOptions {

	public string Host { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public int Port { get; set; } = 0;

}