namespace Neobyte.Cms.Backend.Core.Exceptions.Persistence; 

public class WebsiteAccountAlreadyExistsException : AlreadyExistsException {
	
	public WebsiteAccountAlreadyExistsException (string? message) : base (message) {}
}