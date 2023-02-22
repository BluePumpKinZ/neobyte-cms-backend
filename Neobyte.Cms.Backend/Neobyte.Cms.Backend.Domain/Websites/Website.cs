using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Domain.Websites;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct WebsiteId { }

public class Website {

	public WebsiteId Id { get; set; }
	public string Name { get; set; }
	public string Domain { get; set; }
	public string HomeFolder { get; set; }
	public string UploadFolder { get; set; }
	public DateTime CreatedDate { get; set; }
	public HostingConnection? Connection { get; set; }
	public ICollection<Page>? Pages { get; set; }
	public ICollection<Snippet>? Snippets { get; set; }
	public ICollection<WebsiteAccount>? WebsiteAccounts { get; set; }

	public Website (string name, string domain, string homeFolder, string uploadFolder) : this(WebsiteId.New(), name, domain, homeFolder, uploadFolder, DateTime.UtcNow) {
		Pages = new List<Page>();
		Snippets = new List<Snippet>();
	}

	public Website (WebsiteId id, string name, string domain, string homeFolder, string uploadFolder, DateTime createdDate) {
		Id = id;
		Name = name;
		Domain = domain;
		HomeFolder = homeFolder;
		UploadFolder = uploadFolder;
		CreatedDate = createdDate;
	}

}