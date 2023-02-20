using FluentFTP;
using FluentFTP.Exceptions;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.RemoteHosting.Connections.Connectors;

internal class FluentFtpConnector : RemoteHostingConnector {

	private FtpConnectorOptions? _options;
	private AsyncFtpClient? _client;
	private AsyncFtpClient Client {
		get {
			_client ??= GetFtpClient().Result;
			return _client;
		}
	}

	private async Task<AsyncFtpClient> GetFtpClient () {
		var client = new AsyncFtpClient(_options!.Host, _options!.Username, _options!.Password, _options!.Port);
		await client.Connect();
		return client;
	}

	public override DateTime LastConnectionTime { get; set; } = DateTime.UtcNow;

	public override bool CanConnect (HostingConnection connection) {
		return connection is FtpHostingConnection;
	}

	public override void Configure (HostingConnection connection) {
		var ftpConnection = (FtpHostingConnection)connection;
		_options = new FtpConnectorOptions {
			Host = ftpConnection.Host,
			Username = ftpConnection.Username,
			Password = ftpConnection.Password,
			Port = ftpConnection.Port
		};
	}

	public override async Task<bool> ValidateAsync () {
		try {
			await GetFtpClient();
			return true;
		} catch (FtpAuthenticationException) {
			return false;
		}
	}

	public override async Task<IEnumerable<FilesystemEntry>> ListItemsAsync (string path) {
		var items = await Client.GetListing(path);
		return items.Select(i => new FilesystemEntry(i.Name, path, i.Type == FtpObjectType.Directory, i.Size, i.RawModified));
	}

	public override async Task CreateFolderAsync (string path) {
		await Client.CreateDirectory(path);
	}

	public override async Task RenameFolderAsync (string path, string newPath) {
		await Client.Rename(path, newPath);
	}

	public override async Task DeleteFolderAsync (string path) {
		await Client.DeleteDirectory(path);
	}

	public override async Task CreateFileAsync (string path, byte[] content) {
		await Client.UploadBytes(content, path);
	}

	public override async Task RenameFileAsync (string path, string newPath) {
		await Client.Rename(path, newPath);
	}

	public override async Task DeleteFileAsync (string path) {
		await Client.DeleteFile(path);
	}

	public override async Task<byte[]> GetFileContentAsync (string path) {
		return await Client.DownloadBytes(path, 0);
	}

	public override async Task<bool> FolderExistsAsync (string path) {
		return await Client.DirectoryExists(path);
	}

	public override async Task<bool> FileExistsAsync (string path) {
		return await Client.FileExists(path);
	}

	public override async Task<FilesystemEntry> GetFilesystemEntryInfo (string path) {
		var info = await Client.GetObjectInfo(path, true);
		return new FilesystemEntry(info.Name, info.FullName, info.Type == FtpObjectType.Directory, info.Size, info.RawModified);
	}

	public override bool Equals (object? obj) {
		return obj is FluentFtpConnector connector &&
			   EqualityComparer<FtpConnectorOptions>.Default.Equals(_options, connector._options);
	}

	public override int GetHashCode () {
		// ReSharper disable once NonReadonlyMemberInGetHashCode
		return _options!.GetHashCode();
	}

	public override void Dispose () {
		_client?.Disconnect();
		_client?.Dispose();
	}

}