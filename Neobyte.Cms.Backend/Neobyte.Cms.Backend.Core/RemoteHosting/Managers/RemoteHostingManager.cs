using Neobyte.Cms.Backend.Core.Exceptions.Websites;
using Neobyte.Cms.Backend.Core.Ports.RemoteHosting;
using Neobyte.Cms.Backend.Core.RemoteHosting.Models;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using System;

namespace Neobyte.Cms.Backend.Core.RemoteHosting.Managers;

public class RemoteHostingManager {

	private readonly IRemoteHostingProvider _remoteHostingProvider;

	public RemoteHostingManager (IRemoteHostingProvider remoteHostingProvider) {
		_remoteHostingProvider = remoteHostingProvider;
	}

	public HostingConnection FromRequestModel (RemoteHostingRequestModel request, HostingConnection? existingConnection = null) {
		HostingConnection hostingConnection = Enum.Parse<RemoteHostingRequestModel.HostingProtocol>(request.Protocol) switch {
			RemoteHostingRequestModel.HostingProtocol.FTP => new FtpHostingConnection(
				existingConnection is not null ? new FtpHostingConnectionId(existingConnection.Id.Value) : FtpHostingConnectionId.New(),
				request.Host, request.Username, request.Password, request.Port),
			_ => throw new InvalidProtocolException("Unsupported protocol specified")
		};

		return hostingConnection;
	}

	public async Task<IEnumerable<FilesystemEntry>> ListEntriesAsync (RemoteHostingRequestModel request, string path) {
		var connection = FromRequestModel(request);
		var connector = _remoteHostingProvider.GetConnector(connection);
		return await connector.ListItemsAsync(path);
	}

	public async Task<bool> CheckConnectionAsync (RemoteHostingRequestModel request) {
		var connection = FromRequestModel(request);
		var connector = _remoteHostingProvider.GetConnector(connection);
		return await connector.ValidateAsync();
	}

}