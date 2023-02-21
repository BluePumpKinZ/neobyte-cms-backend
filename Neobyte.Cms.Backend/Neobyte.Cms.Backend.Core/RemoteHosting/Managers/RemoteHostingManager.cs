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

	public async Task<bool> PublicCheckConnectionAsync (RemoteHostingRequestModel request) {
		var connection = FromRequestModel(request);
		return await CheckConnectionAsync(connection);
	}

	private async Task<bool> CheckConnectionAsync (HostingConnection connection) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		return await connector.ValidateAsync();
	}

	private async Task<Website> GetAndValidateWebsiteByIdAsync(WebsiteId id) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(id);
		if (website is null)
			throw new WebsiteNotFoundException($"Website {id} not found.");
		if (website.Connection is null)
			throw new WebsiteConnectionNotFoundException($"Website {id} has no connection");
		return website;
	}

	public async Task PublicAddFolderAsync (RemoteHostingAddFolderRequestModel request) {
		var connection = FromRequestModel(request.Connection);
		await AddFolderAsync(connection, "/", request.Path);
	}

	public async Task HomeAddFolderAsync (WebsiteHomeCreateFolderRequestModel request) {
		var website = await GetAndValidateWebsiteByIdAsync(request.WebsiteId);
		await AddFolderAsync(website.Connection!, website.HomeFolder, request.Path);
	}

	private async Task AddFolderAsync (HostingConnection connection, string relativePath, string path) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		path = _pathUtils.Combine(relativePath, path);
		await connector.CreateFolderAsync(path);
	}

	public async Task<IEnumerable<FilesystemEntry>> PublicListEntriesAsync (RemoteHostingListRequestModel request) {
		var connection = FromRequestModel(request.Connection);
		return await ListEntriesAsync(connection, "/", request.Path);
	}

	public async Task<IEnumerable<FilesystemEntry>> HomeListEntriesAsync (WebsiteHomeListRequestModel request) {
		var website = await GetAndValidateWebsiteByIdAsync(request.WebsiteId);
		return await ListEntriesAsync(website.Connection!, website.HomeFolder, request.Path);
	}

	private async Task<IEnumerable<FilesystemEntry>> ListEntriesAsync (HostingConnection connection, string relativePath, string path) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		string fsPath = _pathUtils.Combine(relativePath, path);
		return await connector.ListItemsAsync(fsPath);
	}

	public async Task PublicRenameFolderAsync (RemoteHostingRenameRequestModel request) {
		var connection = FromRequestModel(request.Connection);
		await RenameFolderAsync(connection, "/", request.Path, request.NewPath);
	}

	public async Task HomeRenameFolderAsync (WebsiteHomeRenameFolderRequestModel request) {
		var website = await GetAndValidateWebsiteByIdAsync(request.WebsiteId);
		await RenameFolderAsync(website.Connection!, website.HomeFolder, request.Path, request.NewPath);
	}

	private async Task RenameFolderAsync (HostingConnection connection, string relativePath, string path, string newPath) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		path = _pathUtils.Combine(relativePath, path);
		newPath = _pathUtils.Combine(relativePath, newPath);
		var entryInfo = await connector.GetFilesystemEntryInfo(path);
		if (entryInfo is null)
			throw new InvalidPathException("The specified path does not exist");
		if (!entryInfo.IsDirectory)
			throw new InvalidPathException("The specified path is not a directory");
		await connector.RenameFolderAsync(path, newPath);
	}

	public async Task HomeDeleteFolderAsync (WebsiteHomeDeleteFolderRequestModel request) {
		var website = await GetAndValidateWebsiteByIdAsync(request.WebsiteId);
		await DeleteFolderAsync(website.Connection!, website.HomeFolder, request.Path);
	}

	private async Task DeleteFolderAsync (HostingConnection connection, string relativePath, string path) {
		var connector = _remoteHostingProvider.GetConnector(connection);
		path = _pathUtils.Combine(relativePath, path);
		var entryInfo = await connector.GetFilesystemEntryInfo(path);
		if (entryInfo is null)
			throw new InvalidPathException("The specified path does not exist");
		if (!entryInfo.IsDirectory)
			throw new InvalidPathException("The specified path is not a directory");
		await connector.DeleteFolderAsync(path);
	}

}