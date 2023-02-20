namespace Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct HostingConnectionId {}

public abstract class HostingConnection {

	public HostingConnectionId Id { get; protected set; }

	protected HostingConnection ()
		: this (HostingConnectionId.New()) { }

	protected HostingConnection (HostingConnectionId id) {
		Id = id;
	}

	public abstract override bool Equals (object? obj);

	public abstract override int GetHashCode ();

}