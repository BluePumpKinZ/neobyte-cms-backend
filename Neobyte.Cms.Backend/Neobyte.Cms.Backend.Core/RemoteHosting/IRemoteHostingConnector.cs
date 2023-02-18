using Neobyte.Cms.Backend.Core.Monitoring;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using System;

namespace Neobyte.Cms.Backend.Core.RemoteHosting;

public interface IRemoteHostingConnector {

	public DateTime LastConnectionTime { get; set; }

	public bool CanConnect (HostingConnection connection);

	public void Configure (HostingConnection connection);

	[Traced]
	public Task<IEnumerable<FilesystemEntry>> ListItemsAsync (string path);

	[Traced]
	public Task CreateFolderAsync (string path);

	[Traced]
	public Task RenameFolderAsync (string path, string newPath);

	[Traced]
	public Task DeleteFolderAsync (string path);

	[Traced]
	public Task CreateFileAsync (string path, byte[] content);

	[Traced]
	public Task RenameFileAsync (string path, string newPath);

	[Traced]
	public Task DeleteFileAsync (string path);

	[Traced]
	public Task<byte[]> GetFileContentAsync (string path);

	[Traced]
	public Task<bool> FolderExistsAsync (string path);

	[Traced]
	public Task<bool> FileExistsAsync (string path);

	public void Dispose ();

}