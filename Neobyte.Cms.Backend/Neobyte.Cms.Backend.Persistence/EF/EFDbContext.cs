using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using Neobyte.Cms.Backend.Persistence.Entities.Accounts;
using Neobyte.Cms.Backend.Persistence.Entities.Websites;
using Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Persistence.EF;

public class EFDbContext : IdentityDbContext<IdentityAccountEntity, IdentityRole<Guid>, Guid> {

	public DbSet<AccountEntity> AccountEntities { get; set; } = null!;
	public DbSet<FtpHostingConnectionEntity> FtpHostingConnectionEntities { get; set; } = null!;
	public DbSet<HtmlContentEntity> HtmlContentEntities { get; set; } = null!;
	public DbSet<HostingConnectionEntity> HostingConnectionEntities { get; set; } = null!;
	public DbSet<PageEntity> PageEntities { get; set; } = null!;
	public DbSet<SnippetEntity> SnippetEntities { get; set; } = null!;
	public DbSet<TemplateEntity> TemplateEntities { get; set; } = null!;
	public DbSet<WebsiteEntity> WebsiteEntities { get; set; } = null!;

	public EFDbContext (DbContextOptions<EFDbContext> options) : base(options) { }

	protected override void OnModelCreating (ModelBuilder modelBuilder) {
		base.OnModelCreating (modelBuilder);

		var accounts = modelBuilder.Entity<AccountEntity>();
		var htmlContents = modelBuilder.Entity<HtmlContentEntity>();
		var hostingConnections = modelBuilder.Entity<HostingConnectionEntity>();
		var pages = modelBuilder.Entity<PageEntity>();
		var snippets = modelBuilder.Entity<SnippetEntity>();
		var templates = modelBuilder.Entity<TemplateEntity>();
		var websites = modelBuilder.Entity<WebsiteEntity>();

		accounts.Property(p => p.Id).HasConversion(v => v.Value, v => new AccountId(v));
		htmlContents.Property(p => p.Id).HasConversion(v => v.Value, v => new HtmlContentId(v));
		hostingConnections.Property(p => p.Id).HasConversion(v => v.Value, v => new HostingConnectionId(v));
		pages.Property(p => p.Id).HasConversion(v => v.Value, v => new PageId(v));
		snippets.Property(p => p.Id).HasConversion(v => v.Value, v => new SnippetId(v));
		templates.Property(p => p.Id).HasConversion(v => v.Value, v => new TemplateId(v));
		websites.Property(p => p.Id).HasConversion(v => v.Value, v => new WebsiteId(v));

	}

}