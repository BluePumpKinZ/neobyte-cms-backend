namespace Neobyte.Cms.Backend.Domain.Websites;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct TemplateId { }

public class Template : Website {

	public string Description { get; set; }
	public string FileName { get; set; }
	public HtmlContent? HtmlContent { get; set; }

	public Template (string name, string description, string fileName, string domain)
		: this(TemplateId.New(), name, description, fileName, domain, DateTime.UtcNow) { }

	public Template (TemplateId id, string name, string description, string fileName, string domain, DateTime createdDate) : base(new WebsiteId(id.Value), name, domain, createdDate) {
		Description = description;
		FileName = fileName;
	}

}