namespace Neobyte.Cms.Backend.Core.Exceptions.Persistence; 

public class PageNotFoundException : NotFoundException {

	public PageNotFoundException (string? message) : base (message) {}

}