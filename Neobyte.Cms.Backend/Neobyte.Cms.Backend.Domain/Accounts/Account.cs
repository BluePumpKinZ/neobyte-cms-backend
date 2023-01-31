namespace Neobyte.Cms.Backend.Domain.Accounts;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct AccountId { }

public class Account {

	[Key]
	public AccountId Id { get; set; }
	[Required]
	[EmailAddress]
	public string Email { get; set; }
	[Required]
	public string Firstname { get; set; }
	[Required]
	public string Lastname { get; set; }
	[Required]
	public string EncodedPassword { get; set; }
	public bool Enabled { get; set; }
	public bool EmailConfirmed { get; set; }
	[Required]
	public DateTime CreationDate { get; set; }
	public ICollection<AccountRole>? AccountRoles { get; set; }

	public Account (string email, string firstname, string lastname, string encodedPassword) {
		Id = AccountId.New();
		Email = email;
		Firstname = firstname;
		Lastname = lastname;
		EncodedPassword = encodedPassword;
		Enabled = true;
		EmailConfirmed = false;
		CreationDate = DateTime.UtcNow;
		AccountRoles = new List<AccountRole>();
	}

	public Account (AccountId id, string email, string firstname, string lastname, string encodedPassword, bool enabled, bool emailConfirmed, DateTime creationDate) {
		Id = id;
		Email = email;
		Firstname = firstname;
		Lastname = lastname;
		EncodedPassword = encodedPassword;
		Enabled = enabled;
		EmailConfirmed = emailConfirmed;
		CreationDate = creationDate;
	}

}