using Limilabs.FTP.Client;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.RemoteHosting.Connections.Connectors;

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

	public Task<IEnumerable<FilesystemEntry>> ListItemsAsync (string path) {
		using var ftp = GetFtp();
		ftp.ChangeFolder(path);
		var items = ftp.GetList();
		ftp.Close();

		var filesystemEntries = items.Select(i => new FilesystemEntry(i.Name, path, i.IsFolder, i.Size, i.ModifyDate));
		return Task.FromResult(filesystemEntries);
	}

	public Task CreateFolderAsync (string path) {
		using var ftp = GetFtp();
		ftp.CreateFolder(path);
		ftp.Close();
		return Task.CompletedTask;
	}

	public Task RenameFolderAsync (string path, string newPath) {
		using var ftp = GetFtp();
		ftp.Rename(path, newPath);
		ftp.Close();
		return Task.CompletedTask;
	}

	public Task DeleteFolderAsync (string path) {
		using var ftp = GetFtp();
		ftp.DeleteFolder(path);
		ftp.Close();
		return Task.CompletedTask;
	}

	public Task CreateFileAsync (string path, byte[] content) {
		using var ftp = GetFtp();
		ftp.Upload(path, content);
		ftp.Close();
		return Task.CompletedTask;
	}

	public Task RenameFileAsync (string path, string newPath) {
		using var ftp = GetFtp();
		ftp.Rename(path, newPath);
		ftp.Close();
		return Task.CompletedTask;
	}

	public Task DeleteFileAsync (string path) {
		using var ftp = GetFtp();
		ftp.DeleteFile(path);
		ftp.Close();
		return Task.CompletedTask;
	}

	public Task<byte[]> GetFileContentAsync (string path) {
		using var ftp = GetFtp();
		var content = ftp.Download(path);
		ftp.Close();
		return Task.FromResult(content);
	}

	public Task<bool> FolderExistsAsync (string path) {
		using var ftp = GetFtp();
		var exists = ftp.FolderExists(path);
		ftp.Close();
		return Task.FromResult(exists);
	}

	public Task<bool> FileExistsAsync (string path) {
		using var ftp = GetFtp();
		var exists = ftp.FileExists(path);
		ftp.Close();
		return Task.FromResult(exists);
	}

}