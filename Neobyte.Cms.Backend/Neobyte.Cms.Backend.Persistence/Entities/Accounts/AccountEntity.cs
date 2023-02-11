using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Persistence.Entities.Accounts;

public class AccountEntity {

	[Key]
	public AccountId Id { get; set; }
	[Required]
	public string Username { get; set; }
	[Required]
	public string Bio { get; set; }
	[Required]
	public DateTime CreationDate { get; set; }

	public AccountEntity (AccountId id, string username, string bio, DateTime creationDate) {
		Id = id;
		Username = username;
		Bio = bio;
		CreationDate = creationDate;
	}


}