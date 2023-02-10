namespace Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct HostingConnectionId {}

public abstract class HostingConnection {

	[Key]
	public HostingConnectionId Id { get; set; }
	[Required]
	public Website? Website { get; set; }

	protected HostingConnection ()
		: this (HostingConnectionId.New()) { }

	protected HostingConnection (HostingConnectionId id) {
		Id = id;
	}

}