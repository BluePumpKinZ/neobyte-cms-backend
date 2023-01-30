namespace Neobyte.Cms.Backend.Api.Endpoints; 

internal interface IApiEndpoints {
	
	public string GroupName { get; }
	public string Path { get; }

	public void RegisterApis (RouteGroupBuilder routes);

}