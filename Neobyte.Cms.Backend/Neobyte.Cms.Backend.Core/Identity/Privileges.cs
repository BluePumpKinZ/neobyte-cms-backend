namespace Neobyte.Cms.Backend.Core.Identity; 

public readonly struct Privileges {

	public static Privileges OwnerPrivilege { get; } = new Privileges("OwnerPrivilege", new Roles[] { Roles.Owner });
	public static Privileges ClientPrivilege { get; } = new Privileges("ClientPrivilege", new Roles[] { Roles.Owner, Roles.Client });
	public static Privileges[] All { get; } = new Privileges[] { OwnerPrivilege, ClientPrivilege };

	public string PrivilegeName { get; }
	public Roles[] PrivilegeRoles { get; }

	private Privileges (string privilegeName, Roles[] roles) {
		PrivilegeName = privilegeName;
		PrivilegeRoles = roles;
	}

}