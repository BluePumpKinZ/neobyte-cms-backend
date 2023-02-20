using Neobyte.Cms.Backend.Core.RemoteHosting.Models;

namespace Neobyte.Cms.Backend.Core.Websites.Models; 

public class WebsiteCreateRequestModel : RemoteHostingRequestModel {

	[Required]
	public string Name { get; set; } = string.Empty;
	[Required]
	[Url]
	public string Domain { get; set; } = string.Empty;
	[Required]
	public string HomeFolder { get; set; } = string.Empty;
	[Required]
	public string UploadFolder { get; set; } = string.Empty;

}