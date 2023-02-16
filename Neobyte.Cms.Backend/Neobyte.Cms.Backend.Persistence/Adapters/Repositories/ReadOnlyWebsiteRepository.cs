using Microsoft.Extensions.Logging;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using Neobyte.Cms.Backend.Persistence.EF;
using Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories;

internal class ReadOnlyWebsiteRepository : IReadOnlyWebsiteRepository {

	private readonly EFDbContext _ctx;
	private readonly ILogger<ReadOnlyWebsiteRepository> _logger;

	public ReadOnlyWebsiteRepository (EFDbContext ctx, ILogger<ReadOnlyWebsiteRepository> logger) {
		_ctx = ctx;
		_logger = logger;
	}

	public async Task<Website?> ReadWebsiteByIdAsync (WebsiteId websiteId) {
		var entity = await _ctx.WebsiteEntities
			.SingleOrDefaultAsync(w => w.Id == websiteId);

		if (entity is null) return null;

		HostingConnection? connection = await ReadWebsiteConnectionByWebsiteIdAsync(websiteId);
		return new Website(entity.Id, entity.Name, entity.Domain, entity.HomeFolder, entity.UploadFolder, entity.CreatedDate) { Connection = connection };
	}

	public async Task<IEnumerable<Website>> ReadAllWebsitesAsync () {
		return await _ctx.WebsiteEntities
			.Select(w => w.ToDomain())
			.ToListAsync();
	}

	private async Task<HostingConnection?> ReadWebsiteConnectionByWebsiteIdAsync (WebsiteId websiteId) {
		var entity = await _ctx.WebsiteEntities.Where(w => w.Id == websiteId)
			.Include(w => w.Connection)
			.Select(w => w.Connection).SingleOrDefaultAsync();
		
		switch (entity) {
		case null: return null;
		case FtpHostingConnectionEntity ftpHostingEntity:
			return new FtpHostingConnection(new FtpHostingConnectionId(ftpHostingEntity.Id.Value),
				ftpHostingEntity.Host, ftpHostingEntity.Username, ftpHostingEntity.Password, ftpHostingEntity.Port);
		default:
			_logger.LogWarning("Website connectionentity for website {websiteId} could no be found", websiteId);
			return null;
		}
	}

}