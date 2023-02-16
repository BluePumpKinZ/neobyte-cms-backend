using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Persistence.Entities.Websites;

[Table("Pages")]
public class PageEntity {

	[Key]
	public PageId Id { get; set; }
	[Required]
	public string Name { get; set; }
	[Required]
	public string Path { get; set; }
	[Required]
	public DateTime Created { get; set; }
	[Required]
	public DateTime Modified { get; set; }
	[Required]
	public WebsiteEntity? Website { get; set; }

	public PageEntity (PageId id, string name, string path, DateTime created, DateTime modified) {
		Id = id;
		Name = name;
		Path = path;
		Created = created;
		Modified = modified;
	}

	internal Page ToDomain () {
		return new Page(Id, Name, Path, Created, Modified);
	}

}