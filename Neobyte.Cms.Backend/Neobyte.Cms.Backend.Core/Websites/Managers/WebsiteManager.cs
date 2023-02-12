using Neobyte.Cms.Backend.Core.Exceptions.Websites;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsiteManager {

	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;
	private readonly IWriteOnlyWebsiteRepository _writeOnlyWebsiteRepository;

	public WebsiteManager (IReadOnlyWebsiteRepository readOnlyWebsiteRepository, IWriteOnlyWebsiteRepository writeOnlyWebsiteRepository) {
		_readOnlyWebsiteRepository = readOnlyWebsiteRepository;
		_writeOnlyWebsiteRepository = writeOnlyWebsiteRepository;
	}

	public async Task AddWebsiteAsync (WebsiteCreateRequestModel request) {
		var website = new Website(request.Name, request.Domain);
		HostingConnection? hostingConnection = Enum.Parse<WebsiteCreateRequestModel.HostingProtocol>(request.Protocol) switch {
			WebsiteCreateRequestModel.HostingProtocol.FTP => new FtpHostingConnection(request.Host, request.Username, request.Password, request.Port),
			WebsiteCreateRequestModel.HostingProtocol.None => null,
			_ => throw new InvalidProtocolException("Unsupported protocol specified")
		};

		website.Connection = hostingConnection;
		await _writeOnlyWebsiteRepository.CreateWebsiteAsync(website);
	}

	public async Task<Website> GetWebsiteById (WebsiteId websiteId) {
		return await _readOnlyWebsiteRepository.GetWebsiteByIdAsync(websiteId);
	}

	public async Task<IEnumerable<Website>> GetAllWebsitesAsync () {
		return await _readOnlyWebsiteRepository.GetAllWebsitesAsync();
	}

}