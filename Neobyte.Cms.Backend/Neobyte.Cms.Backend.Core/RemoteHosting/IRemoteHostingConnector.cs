using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using System;

namespace Neobyte.Cms.Backend.Core.RemoteHosting;

public interface IRemoteHostingConnector {

	public DateTime LastConnectionTime { get; set; }

	public bool CanConnect (HostingConnection connection);

	public void Configure (HostingConnection connection);

	public Task<bool> ValidateAsync ();

	public Task<IEnumerable<FilesystemEntry>> ListItemsAsync (string path);

	public Task CreateFolderAsync (string path);

	public Task RenameFolderAsync (string path, string newPath);

	public Task DeleteFolderAsync (string path);

	public Task CreateFileAsync (string path, byte[] content);

	public Task RenameFileAsync (string path, string newPath);

	public Task DeleteFileAsync (string path);

	public Task<byte[]> GetFileContentAsync (string path);

	public Task<bool> FolderExistsAsync (string path);

	public Task<bool> FileExistsAsync (string path);

	public Task<FilesystemEntry> GetFilesystemEntryInfo (string path);

	public void Disconnect ();

}