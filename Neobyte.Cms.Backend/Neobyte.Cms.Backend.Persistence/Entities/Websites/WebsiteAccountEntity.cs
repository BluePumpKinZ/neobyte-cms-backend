using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Persistence.Entities.Accounts;

namespace Neobyte.Cms.Backend.Persistence.Entities.Websites; 

[Table("WebsiteAccounts")]
public class WebsiteAccountEntity {
	
	[Key]
	public WebsiteAccountId Id { get; set; }
	[Required]
	public DateTime CreatedDate { get; set; }
	[Required]
	public WebsiteEntity? Website { get; set; }
	[Required]
	public AccountEntity? Account { get; set; }
	
	public WebsiteAccountEntity (WebsiteAccountId id, DateTime createdDate) {
		Id = id;
		CreatedDate = createdDate;
	}
	
	public WebsiteAccount ToDomain () {
		return new WebsiteAccount(Id, CreatedDate);
	}
}