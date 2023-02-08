namespace Neobyte.Cms.Backend.Domain.Websites;

[StronglyTypedId (converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct PageId {}

public class Page {

	[Key]
	public PageId Id { get; set; }
	[Required]
	public string Name { get; set; }
	[Required]
	public string Path { get; set; }
	[Required]
	public Website? Website { get; set; }

	public Page (string name, string path)
		: this (PageId.New(), name, path) {}

	public Page (PageId id, string name, string path) {
		Id = id;
		Name = name;
		Path = path;
	}

}