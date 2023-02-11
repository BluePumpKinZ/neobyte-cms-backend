using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Persistence.Entities.Websites;

[Table("Snippets")]
public class SnippetEntity {

	[Key]
	public SnippetId Id { get; set; }
	[Required]
	public string Name { get; set; }
	[Required]
	public string Description { get; set; }
	public string? FileName { get; set; }
	public TemplateEntity? Template { get; set; }
	[Required]
	public HtmlContentEntity? Content { get; set; }

	public SnippetEntity (SnippetId id, string name, string description, string? fileName) {
		Id = id;
		Name = name;
		Description = description;
		FileName = fileName;
	}

}