using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Domain.Websites;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct WebsiteId { }

public class Website {

	[Key]
	public WebsiteId Id { get; set; }
	[Required]
	[StringLength(30)]
	public string Name { get; set; }
	[Required]
	[StringLength(50)]
	public string Domain { get; set; }
	public HostingConnection? Connection { get; set; }
	public ICollection<Page>? Pages { get; set; }
	public ICollection<Snippet>? Snippets { get; set; }

	public Website (string name, string domain) : this(WebsiteId.New(), name, domain) {
		Pages = new List<Page>();
		Snippets = new List<Snippet>();
	}

	public Website (WebsiteId id, string name, string domain) {
		Id = id;
		Name = name;
		Domain = domain;
	}

}