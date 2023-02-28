using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Identity.Initializers;
using Neobyte.Cms.Backend.Persistence.EF;

namespace Neobyte.Cms.Backend.Tests;

[Collection(nameof(IntegrationTests))]
public abstract class IntegrationTests {

	private ProgramApplicationFactory Factory { get; }
	private IServiceProvider Services => Factory.Services;
	private protected IServiceScope ServiceScope => Services.CreateScope();
	private protected HttpClient Client => Factory.CreateClient();
	private protected static Fakers Fakers => new();

	protected IntegrationTests () {
		Factory = new ProgramApplicationFactory();
		ClearDatabase().Wait();
	}

	private async Task ClearDatabase () {
		using var scope = ServiceScope;
		var scopedServices = scope.ServiceProvider;
		var db = scopedServices.GetRequiredService<EFDbContext>();
		await db.Database.EnsureDeletedAsync();
		await db.Database.MigrateAsync();
		var roleIntializer = scopedServices.GetRequiredService<RoleInitializer>();
		await roleIntializer.InitializeRoles();
	}

	private protected async Task<string> OwnerJwtToken () => await CreateAccountAndGetJwtToken(Role.Owner);
	private protected async Task<string> ClientJwtToken () => await CreateAccountAndGetJwtToken(Role.Client);

	private async Task<string> CreateAccountAndGetJwtToken (Role role) {
		using var scope = ServiceScope;
		var scopedServices = scope.ServiceProvider;
		var accountManager = scopedServices.GetRequiredService<AccountManager>();
		var account = Fakers.Accounts.Generate();
		account.Role = role.RoleName;
		var accountId = (await accountManager.CreateAccountWithPasswordAsync(account)).AccountId;
		var identityProvider = scopedServices.GetRequiredService<IIdentityAuthenticationProvider>();
		return (await identityProvider.GenerateJwtTokenAsync(accountId!.Value, true)).token;
	}

}