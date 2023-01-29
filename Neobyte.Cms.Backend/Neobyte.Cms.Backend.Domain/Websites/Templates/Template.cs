namespace Neobyte.Cms.Backend.Domain.Websites.Templates;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct TemplateId { }

public class Template {

	[Key]
	public TemplateId Id { get; set; }
	[Required]
	[StringLength(30)]
	public string Name { get; set; }
	[Required]
	[StringLength(500)]
	public string Description { get; set; }
	public ICollection<Snippet> Snippets { get; set; }

}