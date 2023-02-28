using Neobyte.Cms.Backend.Core.RemoteHosting.Models;

namespace Neobyte.Cms.Backend.Core.RemoteHosting.Managers; 

public class PublicRemoteHostingManager {

	private readonly RemoteHostingManager _remoteHostingManager;

	public PublicRemoteHostingManager (RemoteHostingManager remoteHostingManager) {
		_remoteHostingManager = remoteHostingManager;
	}

	public async Task<bool> PublicCheckConnectionAsync (RemoteHostingRequestModel request) {
		var connection = _remoteHostingManager.FromRequestModel(request);
		return await _remoteHostingManager.CheckConnectionAsync(connection);
	}

	public async Task PublicAddFolderAsync (RemoteHostingAddFolderRequestModel request) {
		var connection = _remoteHostingManager.FromRequestModel(request.Connection);
		await _remoteHostingManager.AddFolderAsync(connection, "/", request.Path);
	}

	public async Task<IEnumerable<FilesystemEntry>> PublicListEntriesAsync (RemoteHostingListRequestModel request) {
		var connection = _remoteHostingManager.FromRequestModel(request.Connection);
		return await _remoteHostingManager.ListEntriesAsync(connection, "/", request.Path);
	}
	
	public async Task PublicRenameFolderAsync (RemoteHostingRenameRequestModel request) {
		var connection = _remoteHostingManager.FromRequestModel(request.Connection);
		await _remoteHostingManager.RenameFolderAsync(connection, "/", request.Path, request.NewPath);
	}

}