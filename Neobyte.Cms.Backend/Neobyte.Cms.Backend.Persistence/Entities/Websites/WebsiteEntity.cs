using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Persistence.Entities.Websites;

[Table("Websites")]
public class WebsiteEntity {

	[Key]
	public WebsiteId Id { get; set; }
	[Required]
	[StringLength(30)]
	public string Name { get; set; }
	[Required]
	[StringLength(50)]
	public string Domain { get; set; }
	[Required]
	public DateTime CreatedDate { get; set; }
	public HostingConnectionEntity? Connection { get; set; }
	public ICollection<PageEntity>? Pages { get; set; }
	public ICollection<SnippetEntity>? Snippets { get; set; }

	public WebsiteEntity (WebsiteId id, string name, string domain, DateTime createdDate) {
		Id = id;
		Name = name;
		Domain = domain;
		CreatedDate = createdDate;
	}

}