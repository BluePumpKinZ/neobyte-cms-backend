namespace Neobyte.Cms.Backend.Domain.Websites;

[StronglyTypedId (converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct PageId {}

public class Page {

	public PageId Id { get; set; }
	public string Name { get; set; }
	public string Path { get; set; }
	public DateTime Created { get; set; }
	public DateTime Modified { get; set; }
	public Website? Website { get; set; }

	public Page (string name, string path)
		: this (PageId.New(), name, path, DateTime.UtcNow, DateTime.UtcNow) {}

	public Page (PageId id, string name, string path, DateTime created, DateTime modified) {
		Id = id;
		Name = name;
		Path = path;
		Created = created;
		Modified = modified;
	}

}