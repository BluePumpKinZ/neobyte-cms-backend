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
	public ICollection<Page> Pages { get; set; }

}