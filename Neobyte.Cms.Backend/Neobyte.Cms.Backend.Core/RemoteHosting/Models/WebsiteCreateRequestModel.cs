using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.RemoteHosting.Models; 

public class WebsiteCreateRequestModel {

	public WebsiteId WebsiteId { get; set; }
	[Required]
	public string Path { get; set; } = string.Empty;

}