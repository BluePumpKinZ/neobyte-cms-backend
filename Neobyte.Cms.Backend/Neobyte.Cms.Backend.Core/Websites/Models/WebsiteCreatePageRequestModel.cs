using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Websites.Models; 

public class WebsiteCreatePageRequestModel {

	public WebsiteId Id { get; set; }
	[Required]
	public string Name { get; set; } = null!;
	[Required]
	public string Path { get; set; } = null!;

}
