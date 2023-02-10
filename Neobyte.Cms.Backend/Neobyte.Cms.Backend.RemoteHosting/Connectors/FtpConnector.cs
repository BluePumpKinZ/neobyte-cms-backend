using Limilabs.FTP.Client;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.RemoteHosting.Connectors;

internal class FtpConnector : IRemoteHostingConnector {

	private readonly FtpConnectorOptions _options = new FtpConnectorOptions();

	private Ftp GetFtp () {
		var ftp = new Ftp();
		ftp.Connect(_options.Host, _options.Port);
		ftp.Login(_options.Username, _options.Password);
		return ftp;
	}

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
		using var ftp = GetFtp();
		ftp.ChangeFolder(path);
		var items = ftp.GetList();
		ftp.Close();
		
		var filesystemEntries = items.Select(i => new FilesystemEntry (i.Name, path, i.IsFolder, i.Size, i.ModifyDate));
		return filesystemEntries;
	}

	public void CreateFolder (string path, string name) {
		using var ftp = GetFtp();
		ftp.ChangeFolder(path);
		ftp.CreateFolder(name);
		ftp.Close();
	}

	public void DeleteFolder (string path) {
		using var ftp = GetFtp();
		ftp.DeleteFolder(path);
		ftp.Close();
	}

	public void CreateFile (string path, string name, byte[] content) {
		using var ftp = GetFtp();
		ftp.ChangeFolder(path);
		ftp.Upload(name, content);
		ftp.Close();
	}

	public void DeleteFile (string path) {
		using var ftp = GetFtp();
		ftp.DeleteFile(path);
		ftp.Close();
	}

	public byte[] GetFileContent (string path) {
		using var ftp = GetFtp();
		var content = ftp.Download(path);
		ftp.Close();
		return content;
	}

}