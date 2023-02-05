namespace Neobyte.Cms.Backend.Core.Identity; 

public readonly struct Role {

	public static Role Owner { get; } = new Role("Owner");
	public static Role Client { get; } = new Role("Client");
	public static Role[] All { get; } = new Role[] { Owner, Client };

	public string RoleName { get; }

	private Role (string roleName) {
		RoleName = roleName;
	}

}