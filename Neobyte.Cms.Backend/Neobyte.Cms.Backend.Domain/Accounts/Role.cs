namespace Neobyte.Cms.Backend.Domain.Accounts;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct RoleId { }

public class Role {

	[Key]
	public RoleId Id { get; set; }
	[Required]
	public string Name { get; set; }
	public ICollection<AccountRole>? AccountRoles { get; set; }

	public Role (string name) {
		Id = RoleId.New();
		Name = name;
		AccountRoles = new List<AccountRole>();
	}

	public Role (RoleId id, string name) {
		Id = id;
		Name = name;
	}

}