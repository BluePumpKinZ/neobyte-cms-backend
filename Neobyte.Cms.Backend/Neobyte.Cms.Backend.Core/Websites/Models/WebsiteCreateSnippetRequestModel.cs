using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Websites.Models; 

public class WebsiteCreateSnippetRequestModel {

	public WebsiteId WebsiteId { get; set; }
	[Required]
	public string Name { get; set; } = null!;
	[Required]
	public string Description { get; set; } = null!;
	[Required]
	public string Content { get; set; } = null!;

}