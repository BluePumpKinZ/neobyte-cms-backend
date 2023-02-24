using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using Neobyte.Cms.Backend.Utils;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Net.Sockets;

namespace Neobyte.Cms.Backend.RemoteHosting.Connections.Connectors;

public class SshNetConnector : IRemoteHostingConnector {

	private readonly PathUtils _pathUtils;
	private readonly SftpConnectorOptions _options = new();
	private SftpClient? _client;

	public SshNetConnector (PathUtils pathUtils) {
		_pathUtils = pathUtils;
	}

	private SftpClient Client {
		get {
			_client ??= GetClient().Result;
			return _client;
		}
	}

	private Task<SftpClient> GetClient () {
		var connectionInfo = new ConnectionInfo(_options.Host, _options.Port, _options.Username,
			new PasswordAuthenticationMethod(_options.Username, _options.Password));
		var client = new SftpClient(connectionInfo);
		client.Connect();
		return Task.FromResult(client);
	}

	public DateTime LastConnectionTime { get; set; } = DateTime.UtcNow;

	public bool CanConnect (HostingConnection connection) {
		return connection is SftpHostingConnection;
	}

	public void Configure (HostingConnection connection) {
		var sftpConnection = (SftpHostingConnection)connection;
		_options.Host = sftpConnection.Host;
		_options.Username = sftpConnection.Username;
		_options.Password = sftpConnection.Password;
		_options.Port = sftpConnection.Port;
	}

	public async Task<bool> ValidateAsync () {
		try {
			var client = await GetClient();
			client.Disconnect();
			return true;
		} catch (Exception e) when (e is SocketException or SshException) {
			return false;
		}
	}

	public async Task<IEnumerable<FilesystemEntry>> ListItemsAsync (string path) {
		var items = await Task.Factory.FromAsync(Client.BeginListDirectory(path, null, null), Client.EndListDirectory);
		items = items.Where(i => !i.Name.StartsWith("."));
		return items.Select(i => new FilesystemEntry(i.Name, i.FullName, i.IsDirectory, i.Length, i.LastWriteTimeUtc));
	}

	public async Task CreateFolderAsync (string path) {
		await Task.Run(() => Client.CreateDirectory(path));
	}

	public async Task RenameFolderAsync (string path, string newPath) {
		await Task.Run(() => Client.RenameFile(path, newPath));
	}

	public async Task DeleteFolderAsync (string path) {
		await Task.Run(() => Client.DeleteDirectory(path));
	}

	public async Task CreateFileAsync (string path, byte[] content) {
		var stream = new MemoryStream(content);
		await Task.Factory.FromAsync(Client.BeginUploadFile(stream, path, null, null), Client.EndUploadFile);
	}

	public async Task RenameFileAsync (string path, string newPath) {
		await Task.Run(() => Client.RenameFile(path, newPath));
	}

	public async Task DeleteFileAsync (string path) {
		await Task.Run(() => Client.DeleteFile(path));
	}

	public async Task<byte[]> GetFileContentAsync (string path) {
		var stream = new MemoryStream();
		await Task.Factory.FromAsync(Client.BeginDownloadFile(path, stream, null, null), Client.EndDownloadFile);
		return stream.ToArray();
	}

	public async Task<bool> FolderExistsAsync (string path) {
		if (!Client.Exists(path))
			return false;
		var info = await GetFilesystemEntryInfo(path);
		return info.IsDirectory;
	}

	public async Task<bool> FileExistsAsync (string path) {
		if (!Client.Exists(path))
			return false;
		var info = await GetFilesystemEntryInfo(path);
		return !info.IsDirectory;
	}

	public async Task<FilesystemEntry> GetFilesystemEntryInfo (string path) {
		var pathAbove = _pathUtils.GetPathAbove(path);
		var files = await ListItemsAsync(pathAbove);
		var file = files.Single(f => f.Path == path);
		return file;
	}

	public void Disconnect () {
		_client?.Disconnect();
		_client?.Dispose();
	}

}