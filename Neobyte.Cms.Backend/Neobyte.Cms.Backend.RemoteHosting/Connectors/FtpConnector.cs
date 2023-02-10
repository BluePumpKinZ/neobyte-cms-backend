using Limilabs.FTP.Client;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.RemoteHosting.Connectors;

internal class FtpConnector : IRemoteHostingConnector {

	private readonly FtpConnectorOptions _options = new FtpConnectorOptions();

	public bool CanConnect (HostingConnection connection) {
		return connection is FtpHostingConnection;
	}

	public void Configure (HostingConnection connection) {
		var ftpConnection = (FtpHostingConnection)connection;
		_options.Host = ftpConnection.Host;
		_options.Username = ftpConnection.Username;
		_options.Password = ftpConnection.Password;
		_options.Port = ftpConnection.Port;
	}

	public IEnumerable<FilesystemEntry> ListItems (string path) {
		using var ftp = new Ftp();
		ftp.Connect(_options.Host);
		ftp.Login(_options.Username, _options.Password);
		ftp.ChangeFolder(path); 
		var items = ftp.GetList();
		ftp.Close();
		
		var filesystemEntries = items.Select(i => new FilesystemEntry (i.Name, path, i.IsFolder, i.Size, i.ModifyDate));
		return filesystemEntries;
	}

	public void CreateFolder (string path, string name) {
		throw new NotImplementedException ();
	}

	public void DeleteFolder (string path) {
		throw new NotImplementedException ();
	}

	public void CreateFile (string path, string name, byte[] content) {
		throw new NotImplementedException ();
	}

	public void DeleteFile (string path) {
		throw new NotImplementedException ();
	}

	public byte[] GetFileContent (string path) {
		throw new NotImplementedException ();
	}

}