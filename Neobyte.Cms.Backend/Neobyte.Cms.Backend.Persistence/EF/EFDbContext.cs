using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Neobyte.Cms.Backend.Domain.Accounts;
using System;

namespace Neobyte.Cms.Backend.Persistence.EF;

public class EFDbContext : IdentityDbContext<IdentityAccount, IdentityRole<Guid>, Guid> {

	public DbSet<Account> Accounts { get; set; } = null!;

	public EFDbContext (DbContextOptions<EFDbContext> options) : base(options) { }

	protected override void OnModelCreating (ModelBuilder modelBuilder) {
		base.OnModelCreating (modelBuilder);

		var accounts = modelBuilder.Entity<Account>();

		accounts.Property(p => p.Id).HasConversion(v => v.Value, v => new AccountId(v));
	}

}