using System.ComponentModel.DataAnnotations;

namespace Neobyte.Cms.Backend.Domain.Websites;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct TemplateId { }

public class Template : Website {

	[Key]
	public new TemplateId Id { get => new (base.Id.Value); set => base.Id = new WebsiteId(value.Value); }
	public string Title { get; set; }
	public string Description { get; set; }

	public Template (string name, string title, string description, string domain, string homeFolder, string uploadFolder)
		: this(TemplateId.New(), name, title, description, domain, homeFolder, uploadFolder, DateTime.UtcNow) { }

	public Template (TemplateId id, string name, string title, string description, string domain, string homeFolder, string uploadFolder, DateTime createdDate) : base(new WebsiteId(id.Value), name, domain, homeFolder, uploadFolder, null, createdDate) {
		Title = title;
		Description = description;
	}

}