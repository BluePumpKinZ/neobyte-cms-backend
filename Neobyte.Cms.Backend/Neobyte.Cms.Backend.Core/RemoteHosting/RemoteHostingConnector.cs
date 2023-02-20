using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using System;

namespace Neobyte.Cms.Backend.Core.RemoteHosting;

public abstract class RemoteHostingConnector {

	public abstract DateTime LastConnectionTime { get; set; }

	public abstract bool CanConnect (HostingConnection connection);

	public abstract void Configure (HostingConnection connection);

	public abstract Task<bool> ValidateAsync ();

	public abstract Task<IEnumerable<FilesystemEntry>> ListItemsAsync (string path);

	public abstract Task CreateFolderAsync (string path);

	public abstract Task RenameFolderAsync (string path, string newPath);

	public abstract Task DeleteFolderAsync (string path);

	public abstract Task CreateFileAsync (string path, byte[] content);

	public abstract Task RenameFileAsync (string path, string newPath);

	public abstract Task DeleteFileAsync (string path);

	public abstract Task<byte[]> GetFileContentAsync (string path);

	public abstract Task<bool> FolderExistsAsync (string path);

	public abstract Task<bool> FileExistsAsync (string path);

	public abstract Task<FilesystemEntry> GetFilesystemEntryInfo (string path);

	public abstract override bool Equals (object? obj);

	public abstract override int GetHashCode ();

	public abstract void Dispose ();

}