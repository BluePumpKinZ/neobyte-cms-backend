using Neobyte.Cms.Backend.Core.RemoteHosting.Models;

namespace Neobyte.Cms.Backend.Core.RemoteHosting.Managers;

public class HomeRemoteHostingManager {

	private readonly RemoteHostingManager _remoteHostingManager;

	public HomeRemoteHostingManager (RemoteHostingManager remoteHostingManager) {
		_remoteHostingManager = remoteHostingManager;
	}

	public async Task HomeAddFolderAsync (WebsiteCreateFolderRequestModel request) {
		var website = await _remoteHostingManager.GetAndValidateWebsiteByIdAsync(request.WebsiteId);
		await _remoteHostingManager.AddFolderAsync(website.Connection!, website.HomeFolder, request.Path);
	}

	public async Task<IEnumerable<FilesystemEntry>> HomeListEntriesAsync (WebsiteListRequestModel request) {
		var website = await _remoteHostingManager.GetAndValidateWebsiteByIdAsync(request.WebsiteId);
		return await _remoteHostingManager.ListEntriesAsync(website.Connection!, website.HomeFolder, request.Path);
	}

	public async Task HomeRenameFolderAsync (WebsiteRenameFolderRequestModel request) {
		var website = await _remoteHostingManager.GetAndValidateWebsiteByIdAsync(request.WebsiteId);
		await _remoteHostingManager.RenameFolderAsync(website.Connection!, website.HomeFolder, request.Path, request.NewPath);
	}

	public async Task HomeDeleteFolderAsync (WebsiteDeleteFolderRequestModel request) {
		var website = await _remoteHostingManager.GetAndValidateWebsiteByIdAsync(request.WebsiteId);
		await _remoteHostingManager.DeleteFolderAsync(website.Connection!, website.HomeFolder, request.Path);
	}


}