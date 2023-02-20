using Neobyte.Cms.Backend.Core.Exceptions.RemoteHosting;
using Neobyte.Cms.Backend.Core.Exceptions.Websites;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Core.RemoteHosting.Models;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using Neobyte.Cms.Backend.Utils;
using System;

namespace Neobyte.Cms.Backend.Core.RemoteHosting.Managers;

public class RemoteHostingManager {

	private readonly IRemoteHostingProvider _remoteHostingProvider;
	private readonly PathUtils _pathUtils;

	public RemoteHostingManager (IRemoteHostingProvider remoteHostingProvider, PathUtils pathUtils) {
		_remoteHostingProvider = remoteHostingProvider;
		_pathUtils = pathUtils;
	}

	public HostingConnection FromRequestModel (RemoteHostingRequestModel request, HostingConnection? existingConnection = null) {
		HostingConnection hostingConnection = Enum.Parse<RemoteHostingRequestModel.HostingProtocol>(request.Protocol) switch {
			RemoteHostingRequestModel.HostingProtocol.FTP => new FtpHostingConnection(
				existingConnection is not null ? new FtpHostingConnectionId(existingConnection.Id.Value) : FtpHostingConnectionId.New(),
				request.Host, request.Username, request.Password, request.Port),
			_ => throw new InvalidProtocolException("Unsupported protocol specified")
		};

		return hostingConnection;
	}

	public async Task<IEnumerable<FilesystemEntry>> PublicListEntriesAsync (RemoteHostingListRequestModel request) {
		var connection = FromRequestModel(request.Connection);
		return await ListEntriesAsync(connection, "/", request.Path);
	}

	private async Task<IEnumerable<FilesystemEntry>> ListEntriesAsync (HostingConnection connection, string relativePath, string path) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		string fsPath = _pathUtils.Combine(relativePath, path);
		return await connector.ListItemsAsync(fsPath);
	}

	public async Task<bool> PublicCheckConnectionAsync (RemoteHostingRequestModel request) {
		var connection = FromRequestModel(request);
		return await CheckConnectionAsync(connection);
	}

	private async Task<bool> CheckConnectionAsync (HostingConnection connection) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		return await connector.ValidateAsync();
	}

	public async Task PublicRenameFolderAsync (RemoteHostingRenameRequestModel request) {
		var connection = FromRequestModel(request.Connection);
		await RenameFolderAsync(connection, request.Path, request.NewPath);
	}

	private async Task RenameFolderAsync (HostingConnection connection, string path, string newPath) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		var entryInfo = await connector.GetFilesystemEntryInfo(path);
		if (!entryInfo.IsDirectory)
			throw new InvalidPathException("The specified path is not a directory");
		await connector.RenameFolderAsync(path, newPath);
	}

}