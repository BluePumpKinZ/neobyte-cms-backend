using Microsoft.Extensions.Logging;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using Neobyte.Cms.Backend.Persistence.EF;
using Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories;

public class ReadOnlyWebsiteRepository : IReadOnlyWebsiteRepository {

	private readonly EFDbContext _ctx;
	private readonly ILogger<ReadOnlyWebsiteRepository> _logger;

	public ReadOnlyWebsiteRepository (EFDbContext ctx, ILogger<ReadOnlyWebsiteRepository> logger) {
		_ctx = ctx;
		_logger = logger;
	}

	public async Task<Website> GetWebsiteByIdAsync (WebsiteId websiteId) {
		var entity = await _ctx.WebsiteEntities
			.Include(w => w.Connection)
			.SingleAsync(w => w.Id == websiteId);

		HostingConnection? connection = await GetWebsiteConnectionByWebsiteIdAsync(websiteId);
		return new Website(entity.Id, entity.Name, entity.Domain, entity.HomeFolder, entity.UploadFolder, entity.CreatedDate) { Connection = connection };
	}

	public async Task<IEnumerable<Website>> GetAllWebsitesAsync () {
		return await _ctx.WebsiteEntities
			.Select(w => new Website(w.Id, w.Name, w.Domain, w.HomeFolder, w.UploadFolder, w.CreatedDate))
			.ToListAsync();
	}

	private async Task<HostingConnection?> GetWebsiteConnectionByWebsiteIdAsync (WebsiteId websiteId) {
		var entity = await _ctx.WebsiteEntities.Where(w => w.Id == websiteId).Select(w => w.Connection).SingleOrDefaultAsync();
		
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