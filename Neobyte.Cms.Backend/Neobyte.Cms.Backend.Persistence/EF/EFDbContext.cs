using Microsoft.EntityFrameworkCore;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Persistence.EF; 

internal class EFDbContext : DbContext {

	public DbSet<Account> Accounts { get; private protected set; } = null!;

	public EFDbContext (DbContextOptions<EFDbContext> options) : base(options) { }

	protected override void OnModelCreating (ModelBuilder modelBuilder) {
		base.OnModelCreating (modelBuilder);

		var accounts = modelBuilder.Entity<Account>();

		accounts.Property(p => p.Id).HasConversion(v => v.Value, v => new AccountId(v));
	}

}