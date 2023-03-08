using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Websites.Models; 

public class WebsiteEditPageRequestModel {

	public WebsiteId WebsiteId { get; set; }
	public PageId PageId { get; set; }
	[Required]
	public string Name { get; set; } = null!;
	[Required]
	public string Path { get; set; } = null!;

}