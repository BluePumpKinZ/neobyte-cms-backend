namespace Neobyte.Cms.Backend.Domain.Websites.Templates;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct SnippetId { }

public class Snippet {

	[Key]
	public SnippetId Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public Template? Template { get; set; }

}