using Microsoft.AspNetCore.Routing;

namespace Neobyte.Cms.Backend.Api.Endpoints; 

internal interface IApiEndpoints {
	
	public string Path { get; }

	public void RegisterApis (RouteGroupBuilder routes);

}