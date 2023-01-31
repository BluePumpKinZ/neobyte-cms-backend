namespace Neobyte.Cms.Backend.Domain.Accounts;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct AccountRoleId { }

public class AccountRole {

	[Key]
	public AccountRoleId Id { get; set; }
	[Required]
	public Account? Account { get; set; }
	[Required]
	public Role? Role { get; set; }

	public AccountRole () {
		Id = AccountRoleId.New();
	}

	public AccountRole (AccountRoleId id) {
		Id = id;
	}

}