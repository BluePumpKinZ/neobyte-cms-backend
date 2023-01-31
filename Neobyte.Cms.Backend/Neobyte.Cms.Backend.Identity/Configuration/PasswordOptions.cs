namespace Neobyte.Cms.Backend.Identity.Configuration; 

public class PasswordOptions {

	public bool RequireDigit { get; set; } = false;
	public bool RequireLowercase { get; set; } = false;
	public bool RequireUppercase { get; set; } = false;
	public bool RequireNonAlphanumeric { get; set; } = false;
	public int RequiredLength { get; set; } = 6;

}