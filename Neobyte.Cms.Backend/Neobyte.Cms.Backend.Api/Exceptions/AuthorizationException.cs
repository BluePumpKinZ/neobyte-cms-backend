namespace Neobyte.Cms.Backend.Api.Exceptions; 

public class AuthorizationException : ApplicationException {

	public AuthorizationException (string? message) : base (message) {}

}