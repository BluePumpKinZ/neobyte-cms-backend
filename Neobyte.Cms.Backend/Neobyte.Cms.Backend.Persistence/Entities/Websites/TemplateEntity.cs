﻿using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Persistence.Entities.Websites;

[Table("Templates")]
public class TemplateEntity {

	[Key]
	public TemplateId Id { get; set; }
	[Required]
	[StringLength(30)]
	public string Title { get; set; }
	[Required]
	[StringLength(500)]
	public string Description { get; set; }
	[Required]
	public DateTime CreateDate { get; set; }
	public HtmlContentEntity? HtmlContent { get; set; }
	public ICollection<PageEntity>? Pages { get; set; }
	public ICollection<SnippetEntity>? Snippets { get; set; }

	public TemplateEntity (TemplateId id, string title, string description, DateTime createDate) {
		Id = id;
		Title = title;
		Description = description;
		CreateDate = createDate;
	}


}