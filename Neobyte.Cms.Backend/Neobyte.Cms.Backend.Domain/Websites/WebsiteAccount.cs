using Neobyte.Cms.Backend.Domain.Accounts;
using System.ComponentModel.DataAnnotations;

namespace Neobyte.Cms.Backend.Domain.Websites;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct WebsiteAccountId { }

public class WebsiteAccount {
	[Key]
	public WebsiteAccountId Id { get; set; }
	public DateTime CreatedDate { get; set; }
	public Website? Website { get; set; }
	public Account? Account { get; set; }
	
	public WebsiteAccount (DateTime createdDate) : this(WebsiteAccountId.New(), createdDate) { }
	
	public WebsiteAccount (WebsiteAccountId id, DateTime createdDate) {
		Id = id;
		CreatedDate = createdDate;
	}
	
	
}