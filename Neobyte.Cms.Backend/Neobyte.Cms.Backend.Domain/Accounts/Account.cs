using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Domain.Accounts;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct AccountId { }

public class Account {

	public AccountId Id { get; set; }
	public string Email { get; set; }
	public string Username { get; set; }
	public string Bio { get; set; }
	public bool Enabled { get; set; }
	public DateTime CreationDate { get; set; }
	public string[] Roles { get; set; }
	public ICollection<WebsiteAccount>? WebsiteAccounts { get; set; }

	public Account (string email, string username, string bio, string[] roles)
		: this(AccountId.New(), email, username, bio, true, DateTime.UtcNow, roles) { }

	public Account (AccountId id, string email, string username, string bio, bool enabled, DateTime creationDate, string[] roles) {
		Id = id;
		Email = email;
		Username = username;
		Bio = bio;
		Enabled = enabled;
		CreationDate = creationDate;
		Roles = roles;
	}

	private bool Equals (Account other) {
		return Id.Equals (other.Id)
			&& Email == other.Email
			&& Username == other.Username
			&& Bio == other.Bio
			&& Enabled == other.Enabled
			&& CreationDate.Equals (other.CreationDate)
			&& Roles.Equals (other.Roles);
	}

	public override bool Equals (object? obj) {
		if (obj is null) {
			return false;
		}

		if (ReferenceEquals (this, obj)) {
			return true;
		}

		if (obj.GetType () != GetType ()) {
			return false;
		}

		return Equals ((Account)obj);
	}

	public override int GetHashCode () {
		// ReSharper disable NonReadonlyMemberInGetHashCode
		return HashCode.Combine (Id, Email, Username, Bio, Enabled, CreationDate, Roles);
	}

}