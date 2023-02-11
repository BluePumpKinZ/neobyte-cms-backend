namespace Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct HostingConnectionId {}

public abstract class HostingConnection {

	public HostingConnectionId Id { get; set; }
	public Website? Website { get; set; }

	protected HostingConnection ()
		: this (HostingConnectionId.New()) { }

	protected HostingConnection (HostingConnectionId id) {
		Id = id;
	}

}