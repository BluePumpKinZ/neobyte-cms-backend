using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Websites.Models; 

public class WebsitePagePublishRequestModel {

	public WebsiteId WebsiteId { get; set; }
	public PageId PageId { get; set; }
	[Required]
	public string Source { get; set; } = string.Empty;

}
