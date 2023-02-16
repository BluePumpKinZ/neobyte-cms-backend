namespace Neobyte.Cms.Backend.Domain.Websites;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct SnippetId { }

public class Snippet {

	public SnippetId Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public Template? Template { get; set; }
	public Website? Website { get; set; }
	public HtmlContent? Content { get; set; }

	public Snippet (string name, string description)
		: this (SnippetId.New(), name, description) {
	}

	public Snippet (SnippetId id, string name, string description) {
		Id = id;
		Name = name;
		Description = description;
	}

}