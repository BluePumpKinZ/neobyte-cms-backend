namespace Neobyte.Cms.Backend.Api.Endpoints.RemoteHosting; 

public class WebsiteRemoteHostingEndpoints : IApiEndpoints {

	public string GroupName => "Anonymous Remote Hosting";
	public string Path => "/api/v1/websites/{websiteId:Guid}/remote-hosting";
	public bool Authorized => true;

	public void RegisterApis (RouteGroupBuilder routes) {
		
	}

}