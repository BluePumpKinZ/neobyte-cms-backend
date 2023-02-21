namespace Neobyte.Cms.Backend.Api.Endpoints.RemoteHosting; 

public class WebsiteUploadRemoteHostingEndpoints : IApiEndpoints {

	public string GroupName => "Website Upload Remote Hosting";
	public string Path => "/api/v1/websites/{websiteId:Guid}/upload";
	public bool Authorized => true;

	public void RegisterApis (RouteGroupBuilder routes) {
		
	}

}