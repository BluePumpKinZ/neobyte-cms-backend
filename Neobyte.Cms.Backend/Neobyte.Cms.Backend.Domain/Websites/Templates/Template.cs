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
	public ICollection<Snippet>? Snippets { get; set; }

	public Template (string name, string description) {
		Id = TemplateId.New();
		Name = name;
		Description = description;
		Snippets = new List<Snippet>();
	}

	public Template (TemplateId id, string name, string description) {
		Id = id;
		Name = name;
		Description = description;
	}

}