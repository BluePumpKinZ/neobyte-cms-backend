namespace Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections;

public class FtpHostingConnectionEntity : HostingConnectionEntity {
	
	[Required]
	public string Host { get; set; }
	[Required]
	public string Username { get; set; }
	[Required]
	public string Password { get; set; }
	[Required]
	public int Port { get; set; }

	public FtpHostingConnectionEntity (string host, string username, string password, int port) {
		Host = host;
		Username = username;
		Password = password;
		Port = port;
	}

}