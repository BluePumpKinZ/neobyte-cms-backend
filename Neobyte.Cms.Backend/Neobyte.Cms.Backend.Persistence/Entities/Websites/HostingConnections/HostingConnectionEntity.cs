using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections;

[Table("HostingConnections")]
public abstract class HostingConnectionEntity {

	[Key]
	public HostingConnectionId Id { get; set; }

	protected HostingConnectionEntity (HostingConnectionId id) {
		Id = id;
	}

	protected HostingConnectionEntity () {}

}