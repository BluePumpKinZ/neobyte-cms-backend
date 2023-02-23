using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Exceptions.RemoteHosting;
using Neobyte.Cms.Backend.Core.Exceptions.Websites;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Core.RemoteHosting.Models;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using Neobyte.Cms.Backend.Utils;
using System;

namespace Neobyte.Cms.Backend.Core.RemoteHosting.Managers;

public class RemoteHostingManager {

	private readonly IRemoteHostingProvider _remoteHostingProvider;
	private readonly PathUtils _pathUtils;
	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;

	public RemoteHostingManager (IRemoteHostingProvider remoteHostingProvider, PathUtils pathUtils, IReadOnlyWebsiteRepository readOnlyWebsiteRepository) {
		_remoteHostingProvider = remoteHostingProvider;
		_pathUtils = pathUtils;
		_readOnlyWebsiteRepository = readOnlyWebsiteRepository;
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

	public async Task<bool> CheckConnectionAsync (HostingConnection connection) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		return await connector.ValidateAsync();
	}

	public async Task<Website> GetAndValidateWebsiteByIdAsync (WebsiteId id) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(id);
		if (website is null)
			throw new WebsiteNotFoundException($"Website {id} not found.");
		if (website.Connection is null)
			throw new WebsiteConnectionNotFoundException($"Website {id} has no connection");
		return website;
	}

	public async Task AddFolderAsync (HostingConnection connection, string relativePath, string path) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		path = _pathUtils.Combine(relativePath, path);
		if (await connector.FolderExistsAsync(path) || await connector.FileExistsAsync(path))
			throw new FolderAlreadyExistsException($"Folder {path} already exists");
		await connector.CreateFolderAsync(path);
	}

	public async Task<IEnumerable<FilesystemEntry>> ListEntriesAsync (HostingConnection connection, string relativePath, string path) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		string fsPath = _pathUtils.Combine(relativePath, path);
		return await connector.ListItemsAsync(fsPath);
	}

	public async Task RenameFolderAsync (HostingConnection connection, string relativePath, string path, string newPath) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		path = _pathUtils.Combine(relativePath, path);
		newPath = _pathUtils.Combine(relativePath, newPath);
		if (!await connector.FolderExistsAsync(path))
			throw new InvalidPathException("The specified folder does not exist");
		var entryInfo = await connector.GetFilesystemEntryInfo(path);
		if (!entryInfo.IsDirectory)
			throw new InvalidPathException("The specified path is not a directory");
		await connector.RenameFolderAsync(path, newPath);
	}

	public async Task DeleteFolderAsync (HostingConnection connection, string relativePath, string path) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		path = _pathUtils.Combine(relativePath, path);
		if (await connector.FolderExistsAsync(path))
			throw new InvalidPathException("The specified folder does not exist");
		var entryInfo = await connector.GetFilesystemEntryInfo(path);
		if (!entryInfo.IsDirectory)
			throw new InvalidPathException("The specified path is not a directory");
		await connector.DeleteFolderAsync(path);
	}

	public async Task RenameFileAsync (HostingConnection connection, string relativePath, string path, string newPath) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		path = _pathUtils.Combine(relativePath, path);
		newPath = _pathUtils.Combine(relativePath, newPath);
		if (!await connector.FileExistsAsync(path))
			throw new InvalidPathException("The specified file does not exist");
		var entryInfo = await connector.GetFilesystemEntryInfo(path);
		if (entryInfo.IsDirectory)
			throw new InvalidPathException("The specified path is not a file");
		await connector.RenameFileAsync(path, newPath);
	}

	public async Task DeleteFileAsync (HostingConnection connection, string relativePath, string path) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		path = _pathUtils.Combine(relativePath, path);
		if (!await connector.FileExistsAsync(path))
			throw new InvalidPathException("The specified file does not exist");
		var entryInfo = await connector.GetFilesystemEntryInfo(path);
		if (entryInfo.IsDirectory)
			throw new InvalidPathException("The specified path is not a file");
		await connector.DeleteFileAsync(path);
	}

}