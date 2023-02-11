using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections;

public abstract class HostingConnectionEntity {

	[Key]
	public HostingConnectionId Id { get; set; }
	[Required]
	public WebsiteEntity? Website { get; set; }

	protected HostingConnectionEntity (HostingConnectionId id) {
		Id = id;
	}

}