using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Persistence.Entities.Websites;

[Table("HtmlContents")]
public class HtmlContentEntity {

	[Key]
	public HtmlContentId Id { get; set; }
	[Required]
	public string Html { get; set; }

	public HtmlContentEntity (HtmlContentId id, string html) {
		Id = id;
		Html = html;
	}

	internal HtmlContent ToDomain () {
		return new HtmlContent(Id, Html);
	}

}