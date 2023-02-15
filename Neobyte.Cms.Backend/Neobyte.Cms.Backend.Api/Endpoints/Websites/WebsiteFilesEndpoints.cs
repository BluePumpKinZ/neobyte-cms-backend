﻿using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Api.Endpoints.Websites; 

public class WebsiteFilesEndpoints : IApiEndpoints {

	public string GroupName => "Website Files";
	public string Path => "api/v1/websites/files";

	public void RegisterApis (RouteGroupBuilder routes) {

		routes.MapGet("list", async (
			[FromServices] IRemoteHostingProvider hostingProvider) => {
				var h = hostingProvider.CreateConnector(new FtpHostingConnection("","","", 21));
				return Results.Ok (h.ListItems("/"));
			}).Authorize(UserPolicy.ClientPrivilege);
		
	}

}