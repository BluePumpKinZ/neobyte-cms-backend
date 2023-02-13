namespace Neobyte.Cms.Backend.Domain.Websites;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct SnippetId { }

public class Snippet {

	public SnippetId Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string? FileName { get; set; }
	public Template? Template { get; set; }
	public HtmlContent? Content { get; set; }

	public Snippet (string name, string description, string? fileName = null)
		: this (SnippetId.New(), name, description, fileName) {
	}

	public Snippet (SnippetId id, string name, string description, string? fileName) {
		Id = id;
		Name = name;
		Description = description;
		FileName = fileName;
	}

}