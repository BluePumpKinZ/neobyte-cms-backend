using FluentFTP;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.RemoteHosting.Connections.Connectors;

internal class FluentFtpConnector : IRemoteHostingConnector {

	private readonly FtpConnectorOptions _options = new();
	private AsyncFtpClient? _client;
	private AsyncFtpClient Client {
		get {
			_client ??= GetFtpClient().Result;
			return _client;
		}
	}

	private async Task<AsyncFtpClient> GetFtpClient () {
		var client = new AsyncFtpClient(_options.Host, _options.Username, _options.Password, _options.Port);
		await client.Connect();
		return client;
	}

	public DateTime LastConnectionTime { get; set; } = DateTime.UtcNow;

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
		var items = await Client.GetListing(path);
		return items.Select(i => new FilesystemEntry(i.Name, path, i.Type == FtpObjectType.Directory, i.Size, i.RawModified));
	}

	public async Task CreateFolderAsync (string path) {
		await Client.CreateDirectory(path);
	}

	public async Task RenameFolderAsync (string path, string newPath) {
		await Client.Rename(path, newPath);
	}

	public async Task DeleteFolderAsync (string path) {
		await Client.DeleteDirectory(path);
	}

	public async Task CreateFileAsync (string path, byte[] content) {
		await Client.UploadBytes(content, path);
	}

	public async Task RenameFileAsync (string path, string newPath) {
		await Client.Rename(path, newPath);
	}

	public async Task DeleteFileAsync (string path) {
		await Client.DeleteFile(path);
	}

	public async Task<byte[]> GetFileContentAsync (string path) {
		return await Client.DownloadBytes(path, 0);
	}

	public async Task<bool> FolderExistsAsync (string path) {
		return await Client.DirectoryExists(path);
	}

	public async Task<bool> FileExistsAsync (string path) {
		return await Client.FileExists(path);
	}

	public void Dispose () {
		_client?.Disconnect();
		_client?.Dispose();
	}

}