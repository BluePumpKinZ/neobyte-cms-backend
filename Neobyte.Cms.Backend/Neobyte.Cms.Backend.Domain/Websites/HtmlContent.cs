namespace Neobyte.Cms.Backend.Domain.Websites;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct HtmlContentId {}

public class HtmlContent {

	[Key]
	public HtmlContentId Id { get; set; }
	public string Html { get; set; }

	public HtmlContent (string html)
		: this(HtmlContentId.New(), html) { }

	public HtmlContent (HtmlContentId id, string html) {
		Id = id;
		Html = html;
	}

}