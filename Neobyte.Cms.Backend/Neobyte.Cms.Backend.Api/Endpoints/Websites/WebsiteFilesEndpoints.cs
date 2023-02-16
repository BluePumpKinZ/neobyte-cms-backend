namespace Neobyte.Cms.Backend.Api.Endpoints.Websites;

internal class WebsiteFilesEndpoints : IApiEndpoints {

	public string GroupName => "Website Files";
	public string Path => "api/v1/websites/files";
	public bool Authorized => true;

	public void RegisterApis (RouteGroupBuilder routes) {

		
		
	}

}