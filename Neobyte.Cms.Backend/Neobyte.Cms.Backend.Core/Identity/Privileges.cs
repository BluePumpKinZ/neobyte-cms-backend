namespace Neobyte.Cms.Backend.Core.Identity; 

public readonly struct Privileges {

	public static Privileges OwnerPrivilege { get; } = new Privileges(new Roles[] { Roles.Owner });
	public static Privileges ClientPrivilege { get; } = new Privileges(new Roles[] { Roles.Owner, Roles.Client });

	public Roles[] PrivilegeRoles { get; }

	private Privileges (Roles[] roles) {
		PrivilegeRoles = roles;
	}

}