using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.RemoteHosting.Models; 

public class WebsiteDeleteRequestModel {

	public WebsiteId WebsiteId { get; set; }
	public string Path { get; set; } = string.Empty;

}