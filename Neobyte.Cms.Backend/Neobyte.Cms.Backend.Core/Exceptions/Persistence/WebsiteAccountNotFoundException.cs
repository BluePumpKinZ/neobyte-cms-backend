namespace Neobyte.Cms.Backend.Core.Exceptions.Persistence; 

public class WebsiteAccountNotFoundException : NotFoundException {
	
	public WebsiteAccountNotFoundException (string? message) : base (message) {}
	
}