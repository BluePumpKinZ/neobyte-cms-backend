using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using System.Collections.Generic;

namespace Neobyte.Cms.Backend.Core.RemoteHosting;

public interface IRemoteHostingConnector {

	public bool CanConnect (HostingConnection connection);

	public void Configure (HostingConnection connection);

	public IEnumerable<FilesystemEntry> ListItems (string path);

	public void CreateFolder (string path, string name);

	public void RenameFolder (string path, string name, string newName);

	public void DeleteFolder (string path);

	public void CreateFile (string path, string name, byte[] content);

	public void RenameFile (string path, string name, string newName);

	public void DeleteFile (string path);

	public byte[] GetFileContent (string path);

}