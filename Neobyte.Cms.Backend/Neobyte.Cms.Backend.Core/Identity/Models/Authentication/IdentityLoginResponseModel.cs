namespace Neobyte.Cms.Backend.Core.Identity.Models.Authentication; 

public class IdentityLoginResponseModel {

	public LoginResult Result { get; set; } = LoginResult.Unknown;

	public enum LoginResult {
		Success,
		LockedOut,
		RequiresTwoFactor,
		Unknown
	}

}