using Microsoft.AspNetCore.Http.Extensions;
using Neobyte.Cms.Backend.Core.Monitoring;

namespace Neobyte.Cms.Backend.Api.Endpoints.Monitoring;

public class MonitoringEndpoints : IApiEndpoints {

	public string GroupName => "Monitoring Dashboard";
	public string Path => "/api/v1/monitoring/dashboard";
	public bool Authorized => false;

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.Map("{*path}", async (
			[FromServices] MonitoringManager manager,
			[FromServices] IHttpContextAccessor httpContextAccessor) => {
				var httpContext = httpContextAccessor.HttpContext!;
				string path = httpContext.Request.GetEncodedPathAndQuery()[Path.Length..];
				if (path.Length == 0)
					path = "/";
				httpContext.Request.Path = path;
				var response = await manager.RelayDashboardRequest(httpContext.Request);

				// Copy request info
				httpContext.Response.ContentType = response.ContentType;
				httpContext.Response.StatusCode = response.StatusCode;

				// copy headers
				foreach (var header in response.Headers)
					httpContext.Response.Headers.Add(header.Key, header.Value);

				// copy body
				await response.Body.CopyToAsync(httpContext.Response.Body);

		});

	}

}