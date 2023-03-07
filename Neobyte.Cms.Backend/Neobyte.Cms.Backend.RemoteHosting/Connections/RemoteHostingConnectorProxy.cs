using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using Neobyte.Cms.Backend.Utils;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Neobyte.Cms.Backend.RemoteHosting.Connections;

public class RemoteHostingConnectorProxy : IRemoteHostingConnector {

	private readonly ActivitySource _activitySource;
	private readonly IRemoteHostingConnector _connector;
	private readonly AsyncUtils _asyncUtils;

	public RemoteHostingConnectorProxy (ActivitySource activitySource, IRemoteHostingConnector connector) {
		_activitySource = activitySource;
		_connector = connector;
		_asyncUtils = new AsyncUtils();
	}

	private Activity? GetActivity ([CallerMemberName] string name = "") {
		var activity = _activitySource.CreateActivity("hosting_connection", ActivityKind.Client);
		activity?.SetTag("hosting_connection.connector", _connector.GetType().Name);
		activity?.SetTag("hosting_connection.method", name);
		activity?.Start();
		return activity;
	}

	public DateTime LastConnectionTime { get => _connector.LastConnectionTime; set { _connector.LastConnectionTime = value; } }

	public bool CanConnect (HostingConnection connection) {
		return _connector.CanConnect(connection);
	}

	public void Configure (HostingConnection connection) {
		_connector.Configure(connection);
	}

	public async Task<bool> ValidateAsync () {
		using var activity = GetActivity();
		return await _asyncUtils.LockAsync(_connector.ValidateAsync);
	}

	public async Task<IEnumerable<FilesystemEntry>> ListItemsAsync (string path) {
		using var activity = GetActivity();
		return await _asyncUtils.LockAsync(() => _connector.ListItemsAsync(path));
	}

	public async Task CreateFolderAsync (string path) {
		using var activity = GetActivity();
		await _asyncUtils.LockAsync(() => _connector.CreateFolderAsync(path));
	}

	public async Task RenameFolderAsync (string path, string newPath) {
		using var activity = GetActivity();
		await _asyncUtils.LockAsync(() => _connector.RenameFolderAsync(path, newPath));
	}

	public async Task DeleteFolderAsync (string path) {
		using var activity = GetActivity();
		await _asyncUtils.LockAsync(() => _connector.DeleteFolderAsync(path));
	}

	public async Task CreateFileAsync (string path, byte[] content) {
		using var activity = GetActivity();
		await _asyncUtils.LockAsync(() => _connector.CreateFileAsync(path, content));
	}

	public async Task RenameFileAsync (string path, string newPath) {
		using var activity = GetActivity();
		await _asyncUtils.LockAsync(() => _connector.RenameFileAsync(path, newPath));
	}

	public async Task DeleteFileAsync (string path) {
		using var activity = GetActivity();
		await _asyncUtils.LockAsync(() => _connector.DeleteFileAsync(path));
	}

	public async Task<byte[]> GetFileContentAsync (string path) {
		using var activity = GetActivity();
		var result = await _asyncUtils.LockAsync(() => _connector.GetFileContentAsync(path));
		return result;
	}

	public async Task<bool> FolderExistsAsync (string path) {
		using var activity = GetActivity();
		return await _asyncUtils.LockAsync(() => _connector.FolderExistsAsync(path));
	}

	public async Task<bool> FileExistsAsync (string path) {
		using var activity = GetActivity();
		return await _asyncUtils.LockAsync(() => _connector.FileExistsAsync(path));
	}

	public async Task<FilesystemEntry> GetFilesystemEntryInfo (string path) {
		using var activity = GetActivity();
		return await _asyncUtils.LockAsync(() => _connector.GetFilesystemEntryInfo(path));
	}

	public void Disconnect () {
		_connector.Disconnect();
	}

}