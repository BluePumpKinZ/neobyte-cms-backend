namespace Neobyte.Cms.Backend.Identity.Configuration;

public class IdentityOptions {

	public PasswordOptions Password { get; set; } = new();
	public SignInOptions SignIn { get; set; } = new();
	public JwtOptions Jwt { get; set; } = new();

}