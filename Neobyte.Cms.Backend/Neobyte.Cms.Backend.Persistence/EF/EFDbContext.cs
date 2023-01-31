using Microsoft.EntityFrameworkCore;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Persistence.EF;

public class EFDbContext : DbContext {

	public DbSet<Account> Accounts { get; set; } = null!;
	public DbSet<AccountRole> AccountRoles { get; set; } = null!;
	public DbSet<Role> Roles { get; set; } = null!;

	public EFDbContext (DbContextOptions<EFDbContext> options) : base(options) { }

	protected override void OnModelCreating (ModelBuilder modelBuilder) {
		base.OnModelCreating (modelBuilder);

		var accounts = modelBuilder.Entity<Account>();
		var accountRoles = modelBuilder.Entity<AccountRole>();
		var roles = modelBuilder.Entity<Role>();

		accounts.Property(p => p.Id).HasConversion(v => v.Value, v => new AccountId(v));
		accountRoles.Property(p => p.Id).HasConversion(v => v.Value, v => new AccountRoleId(v));
		roles.Property(p => p.Id).HasConversion(v => v.Value, v => new RoleId(v));
	}

}