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
	public TemplateEntity? Template { get; set; }
	public WebsiteEntity? Website { get; set; }
	[Required]
	public HtmlContentEntity? Content { get; set; }

	public SnippetEntity (SnippetId id, string name, string description) {
		Id = id;
		Name = name;
		Description = description;
	}

}