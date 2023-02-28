namespace Neobyte.Cms.Backend.Core.RemoteHosting.Models; 

public class RemoteHostingRenameRequestModel {

	[Required]
	public RemoteHostingRequestModel Connection { get; set; } = new();
	[Required]
	public string Path { get; set; } = string.Empty;
	[Required]
	public string NewPath { get; set; } = string.Empty;

}