namespace Neobyte.Cms.Backend.Core.RemoteHosting.Models; 

public class RemoteHostingListRequestModel {

	[Required]
	public RemoteHostingRequestModel Connection { get; set; } = new();
	[Required]
	public string Path { get; set; } = string.Empty;

}