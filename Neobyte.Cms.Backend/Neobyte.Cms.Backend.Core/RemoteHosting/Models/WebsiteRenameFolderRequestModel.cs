using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.RemoteHosting.Models; 

public class WebsiteRenameFolderRequestModel {

	public WebsiteId WebsiteId { get; set; }
	[Required]
	public string Path { get; set; } = string.Empty;
	[Required]
	public string NewPath { get; set; } = string.Empty;

}