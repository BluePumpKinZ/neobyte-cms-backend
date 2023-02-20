namespace Neobyte.Cms.Backend.RemoteHosting.Connections.Connectors;

public class FtpConnectorOptions {

	public string Host { get; init; } = string.Empty;
	public string Username { get; init; } = string.Empty;
	public string Password { get; init; } = string.Empty;
	public int Port { get; init; }

	public override bool Equals (object? obj) {
		if (obj is FtpConnectorOptions options) {
			return options.Host == Host
				&& options.Username == Username
				&& options.Password == Password
				&& options.Port == Port;
		}
		return false;
	}

	public override int GetHashCode () {
		return HashCode.Combine(Host, Username, Password, Port);
	}

}