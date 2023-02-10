namespace Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct FtpHostingConnectionId { }

public class FtpHostingConnection : HostingConnection {

	[Key]
	public new FtpHostingConnectionId Id { get => new (base.Id.Value); set => base.Id = new HostingConnectionId(value.Value); }
	[Required]
	public string Host { get; set; }
	[Required]
	public string Username { get; set; }
	[Required]
	public string Password { get; set; }
	[Required]
	public int Port { get; set; }

	public FtpHostingConnection (FtpHostingConnectionId id, string host, string username, string password, int port)
		: base(new HostingConnectionId(id.Value)) {
		Host = host;
		Username = username;
		Password = password;
		Port = port;
	}

	public FtpHostingConnection (string host, string username, string password, int port) {
		Host = host;
		Username = username;
		Password = password;
		Port = port;
	}

}