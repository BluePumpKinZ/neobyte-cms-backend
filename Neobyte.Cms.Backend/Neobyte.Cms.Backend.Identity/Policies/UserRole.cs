namespace Neobyte.Cms.Backend.Identity.Policies;

public readonly struct UserRole {

	private readonly string _role;

	public static UserRole Owner = new UserRole("Owner");
	public static UserRole Client = new UserRole("Client");

	private UserRole (string role) {
		_role = role;
	}

	public override string ToString () {
		return _role;
	}

	public static implicit operator string (UserRole role) {
		return role._role;
	}

	public static UserRole[] Values { get; } = new UserRole[] { Owner, Client };

}