namespace Neobyte.Cms.Backend.RemoteHosting.Connections.Connectors;

public class SftpConnectorOptions {

	public string Host { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public int Port { get; set; } = 0;

}