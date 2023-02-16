using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Core.RemoteHosting;

public interface IRemoteHostingConnector {

	public bool CanConnect (HostingConnection connection);

	public void Configure (HostingConnection connection);

	public IEnumerable<FilesystemEntry> ListItems (string path);

	public void CreateFolder (string path);

	public void RenameFolder (string path, string newPath);

	public void DeleteFolder (string path);

	public void CreateFile (string path, byte[] content);

	public void RenameFile (string path, string newPath);

	public void DeleteFile (string path);

	public byte[] GetFileContent (string path);

	public bool FolderExists (string path);

	public bool FileExists (string path);

}