using FluentFTP;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.RemoteHosting.Connectors;

public class FluentFtpConnector : IRemoteHostingConnector {

	private readonly FtpConnectorOptions _options = new ();

	private async Task<AsyncFtpClient> GetFtpClient () {
		var client = new AsyncFtpClient(_options.Host, _options.Username, _options.Password, _options.Port);
		await client.Connect();
		return client;
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

	public async Task<IEnumerable<FilesystemEntry>> ListItemsAsync (string path) {
		var ftpClient = await GetFtpClient();
		var items = await ftpClient.GetListing(path);
		await ftpClient.Disconnect();
		return items.Select(i => new FilesystemEntry(i.Name, path, i.Type == FtpObjectType.Directory, i.Size, i.RawModified));
	}

	public async Task CreateFolderAsync (string path) {
		var ftpClient = await GetFtpClient();
		await ftpClient.CreateDirectory(path);
		await ftpClient.Disconnect();
	}

	public async Task RenameFolderAsync (string path, string newPath) {
		var ftpClient = await GetFtpClient();
		await ftpClient.Rename(path, newPath);
		await ftpClient.Disconnect();
	}

	public async Task DeleteFolderAsync (string path) {
		var ftpClient = await GetFtpClient();
		await ftpClient.DeleteDirectory(path);
		await ftpClient.Disconnect();
	}

	public async Task CreateFileAsync (string path, byte[] content) {
		var ftpClient = await GetFtpClient();
		await ftpClient.UploadBytes(content, path);
		await ftpClient.Disconnect();
	}

	public async Task RenameFileAsync (string path, string newPath) {
		var ftpClient = await GetFtpClient();
		await ftpClient.Rename(path, newPath);
		await ftpClient.Disconnect();
	}

	public async Task DeleteFileAsync (string path) {
		var ftpClient = await GetFtpClient();
		await ftpClient.DeleteFile(path);
		await ftpClient.Disconnect();
	}

	public async Task<byte[]> GetFileContentAsync (string path) {
		var ftpClient = await GetFtpClient();
		var content = await ftpClient.DownloadBytes(path, 0);
		await ftpClient.Disconnect();
		return content;
	}

	public async Task<bool> FolderExistsAsync (string path) {
		var ftpClient = await GetFtpClient();
		var exists = await ftpClient.DirectoryExists(path);
		await ftpClient.Disconnect();
		return exists;
	}

	public async Task<bool> FileExistsAsync (string path) {
		var ftpClient = await GetFtpClient();
		var exists = await ftpClient.FileExists(path);
		await ftpClient.Disconnect();
		return exists;
	}

}