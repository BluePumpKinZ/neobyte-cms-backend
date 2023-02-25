using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Identity.Initializers;
using Neobyte.Cms.Backend.Persistence.EF;

namespace Neobyte.Cms.Backend.Tests;

public abstract class IntegrationTests {

	private protected ProgramApplicationFactory Factory { get; }
	private protected IServiceProvider Services => Factory.Services;
	private protected IServiceScope ServiceScope => Services.CreateScope();
	private protected HttpClient Client => Factory.CreateClient();

	protected IntegrationTests () {
		Factory = new ProgramApplicationFactory();
		ClearDatabase().Wait();
	}

	private protected async Task ClearDatabase () {
		using var scope = ServiceScope;
		var scopedServices = scope.ServiceProvider;
		var db = scopedServices.GetRequiredService<EFDbContext>();
		await db.Database.EnsureDeletedAsync();
		await db.Database.MigrateAsync();
		var roleIntializer = scopedServices.GetRequiredService<RoleInitializer>();
		await roleIntializer.InitializeRoles();
	}

}