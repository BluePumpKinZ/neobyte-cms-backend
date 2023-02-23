using Neobyte.Cms.Backend.Core.RemoteHosting.Models;

namespace Neobyte.Cms.Backend.Core.RemoteHosting.Managers;

public class UploadRemoteHostingManager {

	private readonly RemoteHostingManager _remoteHostingManager;

	public UploadRemoteHostingManager (RemoteHostingManager remoteHostingManager) {
		_remoteHostingManager = remoteHostingManager;
	}

	public async Task UploadAddFolderAsync (WebsiteCreateFolderRequestModel request) {
		var website = await _remoteHostingManager.GetAndValidateWebsiteByIdAsync(request.WebsiteId);
		await _remoteHostingManager.AddFolderAsync(website.Connection!, website.UploadFolder, request.Path);
	}

	public async Task<IEnumerable<FilesystemEntry>> UploadListEntriesAsync (WebsiteListRequestModel request) {
		var website = await _remoteHostingManager.GetAndValidateWebsiteByIdAsync(request.WebsiteId);
		return await _remoteHostingManager.ListEntriesAsync(website.Connection!, website.UploadFolder, request.Path);
	}

	public async Task UploadRenameFolderAsync (WebsiteRenameFolderRequestModel request) {
		var website = await _remoteHostingManager.GetAndValidateWebsiteByIdAsync(request.WebsiteId);
		await _remoteHostingManager.RenameFolderAsync(website.Connection!, website.UploadFolder, request.Path, request.NewPath);
	}

	public async Task UploadDeleteFolderAsync (WebsiteDeleteFolderRequestModel request) {
		var website = await _remoteHostingManager.GetAndValidateWebsiteByIdAsync(request.WebsiteId);
		await _remoteHostingManager.DeleteFolderAsync(website.Connection!, website.UploadFolder, request.Path);
	}

}