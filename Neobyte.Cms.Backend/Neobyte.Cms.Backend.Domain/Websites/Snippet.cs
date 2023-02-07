namespace Neobyte.Cms.Backend.Domain.Websites.Templates;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct SnippetId { }

public class Snippet {

	[Key]
	public SnippetId Id { get; set; }
	[Required]
	public string Name { get; set; }
	[Required]
	public string Description { get; set; }
	public Template? Template { get; set; }
	[Required]
	public HtmlContent? Content { get; set; }

	public Snippet (string name, string description) {
		Id = SnippetId.New();
		Name = name;
		Description = description;
	}

	public Snippet (SnippetId id, string name, string description) {
		Id = id;
		Name = name;
		Description = description;
	}

}