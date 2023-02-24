using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using Neobyte.Cms.Backend.Persistence.EF;
using Neobyte.Cms.Backend.Persistence.Entities.Websites;
using Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Persistence.Adapters.Repositories;

internal class WriteOnlyWebsiteRepository : IWriteOnlyWebsiteRepository {

	private readonly EFDbContext _ctx;

	public WriteOnlyWebsiteRepository (EFDbContext ctx) {
		_ctx = ctx;
	}

	public async Task<Website> CreateWebsiteAsync (Website website) {

		var hostingConnection = CreateHostingConnectionEntity(website);
		var entity = new WebsiteEntity(website.Id, website.Name, website.Domain, website.HomeFolder, website.UploadFolder, website.CreatedDate) { Connection = hostingConnection };
		var addedEntity = await _ctx.WebsiteEntities.AddAsync(entity);
		await _ctx.SaveChangesAsync();
		website.Id = addedEntity.Entity.Id;
		return website;
	}

	private static HostingConnectionEntity? CreateHostingConnectionEntity (Website website) {
		HostingConnectionEntity? hostingConnection = null;
		switch (website.Connection?.GetType()) {
		case var value when value == typeof(FtpHostingConnection):
			var ftpHostingConnection = website.Connection as FtpHostingConnection;
			hostingConnection = new FtpHostingConnectionEntity(
				new HostingConnectionId(ftpHostingConnection!.Id.Value),
				ftpHostingConnection.Host,
				ftpHostingConnection.Username,
				ftpHostingConnection.Password,
				ftpHostingConnection.Port);
			break;
		case var value when value == typeof(SftpHostingConnection):
			var sftpHostingConnection = website.Connection as SftpHostingConnection;
			hostingConnection = new SftpHostingConnectionEntity(
				new HostingConnectionId(sftpHostingConnection!.Id.Value),
				sftpHostingConnection.Host,
				sftpHostingConnection.Username,
				sftpHostingConnection.Password,
				sftpHostingConnection.Port);
			break;
		case null:
			hostingConnection = null;
			break;
		}
		return hostingConnection;
	}

	public async Task<Website> UpdateWebsiteAsync (Website website) {

		var existingConnection = await _ctx.HostingConnectionEntities.SingleOrDefaultAsync(x => x.Id == website.Connection!.Id);
		if (existingConnection is not null)
			_ctx.HostingConnectionEntities.Remove(existingConnection);

		HostingConnectionEntity? hostingConnectionEntity = CreateHostingConnectionEntity(website);
		var entity = await _ctx.WebsiteEntities.SingleAsync(w => w.Id == website.Id);
		entity.Name = website.Name;
		entity.Domain = website.Domain;
		entity.HomeFolder = website.HomeFolder;
		entity.UploadFolder = website.UploadFolder;
		entity.Connection = hostingConnectionEntity;
		_ctx.WebsiteEntities.Update(entity);
		await _ctx.SaveChangesAsync();
		return website;
	}
}