namespace Neobyte.Cms.Backend.Core.Identity; 

public readonly struct UserPolicy {

	public static UserPolicy OwnerPrivilege { get; } = new UserPolicy("OwnerPrivilege", new [] { Role.Owner });
	public static UserPolicy ClientPrivilege { get; } = new UserPolicy("ClientPrivilege", new [] { Role.Owner, Role.Client });
	public static UserPolicy[] All { get; } = new UserPolicy[] { OwnerPrivilege, ClientPrivilege };

	public string Name { get; }
	public Role[] Roles { get; }

	private UserPolicy (string name, Role[] roles) {
		Name = name;
		Roles = roles;
	}

}