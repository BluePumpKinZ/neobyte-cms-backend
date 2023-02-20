using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Neobyte.Cms.Backend.RemoteHosting.Connections;

internal class RemoteHostingConnectorProxy : RemoteHostingConnector {

	private readonly ActivitySource _activitySource;
	private readonly RemoteHostingConnector _connector;

	public RemoteHostingConnectorProxy (ActivitySource activitySource, RemoteHostingConnector connector) {
		_activitySource = activitySource;
		_connector = connector;
	}

	private Activity? GetActivity ([CallerMemberName] string name = "") {
		var activity = _activitySource.CreateActivity("hosting_connection", ActivityKind.Client);
		activity?.SetTag("hosting_connection.connector", _connector.GetType().Name);
		activity?.SetTag("hosting_connection.method", name);
		activity?.Start();
		return activity;
	}

	public override DateTime LastConnectionTime { get => _connector.LastConnectionTime; set { _connector.LastConnectionTime = value; } }

	public override bool CanConnect (HostingConnection connection) {
		return _connector.CanConnect(connection);
	}

	public override void Configure (HostingConnection connection) {
		_connector.Configure(connection);
	}

	public override async Task<bool> ValidateAsync () {
		using var activity = GetActivity();
		return await _connector.ValidateAsync();
	}

	public override async Task<IEnumerable<FilesystemEntry>> ListItemsAsync (string path) {
		using var activity = GetActivity();
		return await _connector.ListItemsAsync(path);
	}

	public override async Task CreateFolderAsync (string path) {
		using var activity = GetActivity();
		await _connector.CreateFolderAsync(path);
	}

	public override async Task RenameFolderAsync (string path, string newPath) {
		using var activity = GetActivity();
		await _connector.RenameFolderAsync(path, newPath);
	}

	public override async Task DeleteFolderAsync (string path) {
		using var activity = GetActivity();
		await _connector.DeleteFolderAsync(path);
	}

	public override async Task CreateFileAsync (string path, byte[] content) {
		using var activity = GetActivity();
		await _connector.CreateFileAsync(path, content);
	}

	public override async Task RenameFileAsync (string path, string newPath) {
		using var activity = GetActivity();
		await _connector.RenameFileAsync(path, newPath);
	}

	public override async Task DeleteFileAsync (string path) {
		using var activity = GetActivity();
		await _connector.DeleteFileAsync(path);
	}

	public override async Task<byte[]> GetFileContentAsync (string path) {
		using var activity = GetActivity();
		var result = await _connector.GetFileContentAsync(path);
		return result;
	}

	public override async Task<bool> FolderExistsAsync (string path) {
		using var activity = GetActivity();
		return await _connector.FolderExistsAsync(path);
	}

	public override async Task<bool> FileExistsAsync (string path) {
		using var activity = GetActivity();
		return await _connector.FileExistsAsync(path);
	}

	public override async Task<FilesystemEntry> GetFilesystemEntryInfo (string path) {
		using var activity = GetActivity();
		return await _connector.GetFilesystemEntryInfo(path);
	}

	public override bool Equals (object? obj) {
		return _connector.Equals(obj);
	}

	public override int GetHashCode () {
		return _connector.GetHashCode();
	}

	public override void Dispose () {
		_connector.Dispose();
	}

}