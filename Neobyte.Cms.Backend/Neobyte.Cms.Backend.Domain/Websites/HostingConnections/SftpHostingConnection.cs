﻿namespace Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct SftpHostingConnectionId { }

public class SftpHostingConnection : HostingConnection {

	public new SftpHostingConnectionId Id { get => new(base.Id.Value); set => base.Id = new HostingConnectionId(value.Value); }

	public string Host { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public int Port { get; set; }

	public SftpHostingConnection (SftpHostingConnectionId id, string host, string username, string password, int port)
		: base(new HostingConnectionId(id.Value)) {
		Host = host;
		Username = username;
		Password = password;
		Port = port;
	}

	public SftpHostingConnection (string host, string username, string password, int port) {
		Host = host;
		Username = username;
		Password = password;
		Port = port;
	}

	public override bool Equals (object? obj) {
		return obj is SftpHostingConnection connection &&
			   Host == connection.Host &&
			   Username == connection.Username &&
			   Password == connection.Password &&
			   Port == connection.Port;
	}

	public override int GetHashCode () {
		// ReSharper disable NonReadonlyMemberInGetHashCode
		return HashCode.Combine(Host, Username, Password, Port);
	}

}