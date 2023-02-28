namespace Neobyte.Cms.Backend.Core.RemoteHosting.Models; 

public class RemoteHostingAddFolderRequestModel {

	public RemoteHostingRequestModel Connection { get; set; } = new();
	public string Path { get; set; } = string.Empty;

}