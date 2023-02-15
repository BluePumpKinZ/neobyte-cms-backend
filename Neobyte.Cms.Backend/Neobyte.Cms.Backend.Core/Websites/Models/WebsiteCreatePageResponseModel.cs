namespace Neobyte.Cms.Backend.Core.Websites.Models; 

public class WebsiteCreatePageResponseModel {

	public bool Success { get; }
	public string[]? Errors { get; }

	public WebsiteCreatePageResponseModel (bool success) {
		Success = success;
	}

	public WebsiteCreatePageResponseModel (bool success, string[]? errors) {
		Success = success;
		Errors = errors;
	}

}