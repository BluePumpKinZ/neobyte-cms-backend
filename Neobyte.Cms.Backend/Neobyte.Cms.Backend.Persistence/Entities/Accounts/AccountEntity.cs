using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Persistence.Entities.Websites;

namespace Neobyte.Cms.Backend.Persistence.Entities.Accounts;

[Table("Accounts")]
public class AccountEntity {

	[Key]
	public AccountId Id { get; set; }
	[Required]
	public string Username { get; set; }
	[Required]
	public string Bio { get; set; }
	[Required]
	public bool Enabled { get; set; }
	[Required]
	public DateTime CreationDate { get; set; }
	public ICollection<WebsiteAccountEntity>? WebsiteAccounts { get; set; }

	public AccountEntity (AccountId id, string username, string bio, bool enabled, DateTime creationDate) {
		Id = id;
		Username = username;
		Bio = bio;
		Enabled = enabled;
		CreationDate = creationDate;
	}
}