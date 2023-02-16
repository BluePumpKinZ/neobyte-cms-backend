using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Exceptions.Websites;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using System;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsiteManager {

	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;
	private readonly IWriteOnlyWebsiteRepository _writeOnlyWebsiteRepository;

	public WebsiteManager (IReadOnlyWebsiteRepository readOnlyWebsiteRepository, IWriteOnlyWebsiteRepository writeOnlyWebsiteRepository) {
		_readOnlyWebsiteRepository = readOnlyWebsiteRepository;
		_writeOnlyWebsiteRepository = writeOnlyWebsiteRepository;
	}

	public async Task<Website> AddWebsiteAsync (WebsiteCreateRequestModel request) {
		var website = new Website(request.Name, request.Domain, request.HomeFolder, request.UploadFolder);
		HostingConnection? hostingConnection = Enum.Parse<WebsiteCreateRequestModel.HostingProtocol>(request.Protocol) switch {
			WebsiteCreateRequestModel.HostingProtocol.FTP => new FtpHostingConnection(request.Host, request.Username, request.Password, request.Port),
			WebsiteCreateRequestModel.HostingProtocol.None => null,
			_ => throw new InvalidProtocolException("Unsupported protocol specified")
		};

		website.Connection = hostingConnection;
		return await _writeOnlyWebsiteRepository.CreateWebsiteAsync(website);
	}

	public async Task<Website> GetWebsiteById (WebsiteId websiteId) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId);
		if (website is null)
			throw new WebsiteNotFoundException($"Website {websiteId} not found");
		return website;
	}

	public async Task<IEnumerable<Website>> GetAllWebsitesAsync () {
		return await _readOnlyWebsiteRepository.ReadAllWebsitesAsync();
	}

	public async Task<Website> EditWebsiteAsync (WebsiteEditRequestModel request) {

		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(new WebsiteId(request.Id));

		if (website is null)
			throw new WebsiteNotFoundException($"Website {request.Id} not found");

		website.Name = request.Name;
		website.Domain = request.Domain;
		website.HomeFolder = request.HomeFolder;
		website.UploadFolder = request.UploadFolder;

		HostingConnection? hostingConnection = Enum.Parse<WebsiteCreateRequestModel.HostingProtocol>(request.Protocol) switch {
			WebsiteCreateRequestModel.HostingProtocol.FTP => new FtpHostingConnection(
				website.Connection is not null ? new FtpHostingConnectionId(website.Connection!.Id.Value) : FtpHostingConnectionId.New(),
				request.Host, request.Username, request.Password, request.Port),
			WebsiteCreateRequestModel.HostingProtocol.None => null,
			_ => throw new InvalidProtocolException("Unsupported protocol specified")
		};

		website.Connection = hostingConnection;

		return await _writeOnlyWebsiteRepository.UpdateWebsiteAsync(website);
	}

}