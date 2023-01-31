namespace Neobyte.Cms.Backend.Identity.Authentication.Passwords; 

public interface IPasswordRule {

	public bool Applies ();

	public bool Validate (string password, out string? error);

}