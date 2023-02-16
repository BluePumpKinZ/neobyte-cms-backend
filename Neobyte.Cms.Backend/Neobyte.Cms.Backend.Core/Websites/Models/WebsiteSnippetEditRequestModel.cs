using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Websites.Models; 

public class WebsiteSnippetEditRequestModel : WebsiteCreateSnippetRequestModel {

	public SnippetId SnippetId { get; set; }

}