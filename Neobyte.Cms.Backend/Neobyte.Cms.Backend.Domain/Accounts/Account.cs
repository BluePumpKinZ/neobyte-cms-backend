namespace Neobyte.Cms.Backend.Domain.Accounts;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct AccountId { }

public class Account {

	public AccountId Id { get; set; }
	public string Email { get; set; }
	public string Username { get; set; }
	public string Bio { get; set; }
	public DateTime CreationDate { get; set; }
	public string[] Roles { get; set; }

	public Account (string email, string username, string bio, string[] roles)
		: this(AccountId.New(), email, username, bio, DateTime.UtcNow, roles) { }

	public Account (AccountId id, string email, string username, string bio, DateTime creationDate, string[] roles) {
		Id = id;
		Email = email;
		Username = username;
		Bio = bio;
		CreationDate = creationDate;
		Roles = roles;
	}

}