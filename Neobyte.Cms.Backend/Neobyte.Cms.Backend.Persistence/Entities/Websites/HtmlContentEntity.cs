using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Persistence.Entities.Websites;

public class HtmlContentEntity {

	[Key]
	public HtmlContentId Id { get; set; }
	public string Html { get; set; }

	public HtmlContentEntity (HtmlContentId id, string html) {
		Id = id;
		Html = html;
	}

}