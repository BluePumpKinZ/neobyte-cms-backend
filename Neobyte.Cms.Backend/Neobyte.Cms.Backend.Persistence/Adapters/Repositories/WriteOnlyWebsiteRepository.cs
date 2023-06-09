﻿using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
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
		var entity = new WebsiteEntity(website.Id, website.Name, website.Domain, website.HomeFolder, website.UploadFolder, website.Thumbnail, website.CreatedDate) { Connection = hostingConnection };
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
				HostingConnectionId.New(),
				ftpHostingConnection!.Host,
				ftpHostingConnection.Username,
				ftpHostingConnection.Password,
				ftpHostingConnection.Port);
			break;
		case var value when value == typeof(SftpHostingConnection):
			var sftpHostingConnection = website.Connection as SftpHostingConnection;
			hostingConnection = new SftpHostingConnectionEntity(
				HostingConnectionId.New(),
				sftpHostingConnection!.Host,
				sftpHostingConnection.Username,
				sftpHostingConnection.Password,
				sftpHostingConnection.Port);
			break;
		case var value when value == typeof(S3HostingConnection):
			var s3HostingConnection = website.Connection as S3HostingConnection;
			hostingConnection = new S3HostingConnectionEntity(
				HostingConnectionId.New(),
				s3HostingConnection!.AccessKey,
				s3HostingConnection.SecretKey,
				s3HostingConnection.BucketName,
				s3HostingConnection.Region);
			break;
		case null:
			hostingConnection = null;
			break;
		}
		return hostingConnection;
	}

	public async Task<Website> UpdateWebsiteAsync (Website website) {

		if (website.Connection is not null) {
			var existingConnection = await _ctx.HostingConnectionEntities.SingleAsync(x => x.Id == website.Connection!.Id);
			_ctx.HostingConnectionEntities.Remove(existingConnection);
		}

		var hostingConnectionEntity = CreateHostingConnectionEntity(website);
		if (hostingConnectionEntity is not null)
			hostingConnectionEntity = (await _ctx.HostingConnectionEntities.AddAsync(hostingConnectionEntity)).Entity;
		var entity = await _ctx.WebsiteEntities.SingleAsync(w => w.Id == website.Id);
		entity.Name = website.Name;
		entity.Domain = website.Domain;
		entity.HomeFolder = website.HomeFolder;
		entity.UploadFolder = website.UploadFolder;
		entity.Thumbnail = website.Thumbnail;
		entity.Connection = hostingConnectionEntity;
		_ctx.WebsiteEntities.Update(entity);
		await _ctx.SaveChangesAsync();
		return website;
	}

	public async Task DeleteWebsiteAsync (Website website) {

		if (website.Connection is not null) {
			var existingConnection = await _ctx.HostingConnectionEntities.SingleAsync(x => x.Id == website.Connection!.Id);
			_ctx.HostingConnectionEntities.Remove(existingConnection);
		}

		var entity = await _ctx.WebsiteEntities.SingleAsync(w => w.Id == website.Id);
		_ctx.WebsiteEntities.Remove(entity);
		await _ctx.SaveChangesAsync();
	}

}