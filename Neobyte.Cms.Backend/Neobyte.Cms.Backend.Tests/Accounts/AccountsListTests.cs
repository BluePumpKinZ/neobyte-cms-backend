using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Api.Projections.Projections;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
using System.Net;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Identity;

namespace Neobyte.Cms.Backend.Tests.Accounts;

public class AccountsListTests : IntegrationTests {

	[Fact]
	public async Task All_ShouldMapAllProperties () {

		// Arrange
		using var scope = ServiceScope;
		var accountManager = scope.ServiceProvider.GetRequiredService<AccountManager>();
		var accountDetails = Fakers.Accounts.Generate();
		var accountId = (await accountManager.CreateAccountWithPasswordAsync(accountDetails)).AccountId!.Value;

		// Act
		var response = await Client.Authorize(await OwnerJwtToken())
			.GetAsync("/api/v1/accounts/list/all");

		// Assert
		response.EnsureSuccessStatusCode();
		var accounts = (await response.Content.ReadFromJsonAsync<AccountProjection[]>())!;
		Assert.True(accounts.Any());
		Assert.Contains(accounts, a => a.Id == accountId);
		var accountProjection = accounts.First(a => a.Id == accountId);
		Assert.Equal(accountDetails.Email, accountProjection.Email);
		Assert.Equal(accountDetails.Username, accountProjection.Username);
		Assert.Equal(accountDetails.Bio, accountProjection.Bio);
		Assert.Equal(accountDetails.Role, accountProjection.Roles?[0]);
		Assert.True(accountProjection.Enabled);
	}

	[Fact]
	public async Task All_ShouldNotWork_IfClientAuthorized () {

		// Act
		var response = await Client.Authorize(await ClientJwtToken())
			.GetAsync("/api/v1/accounts/list/all");

		// Assert
		Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

	}

	[Fact]
	public async Task All_ShouldNotWork_IfNotAuthorized () {

		// Act
		var response = await Client.GetAsync("/api/v1/accounts/list/all");

		// Assert
		Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

	}

	[Fact]
	public async Task CreateAccountWithPassword_ShouldCreateAccount() {

		// Arrange
		using var scope = ServiceScope;
		var accountManager = scope.ServiceProvider.GetRequiredService<AccountManager>();
		var accountDetails = Fakers.Accounts.Generate();

		// Act
		var response = await Client.Authorize(await OwnerJwtToken())
			.PostAsJsonAsync("/api/v1/accounts/list/create/with-password", accountDetails);

		// Assert
		response.EnsureSuccessStatusCode();

		var r = new { Message = "", Id = Guid.Empty };

		dynamic result = (await response.Content.ReadFromJsonAsync(r.GetType()))!;
		Assert.NotEqual(Guid.Empty, result.Id);
		var account = await accountManager.GetAccountDetailsAsync(new AccountId (result.Id));
		Assert.NotNull(account);
		Assert.Equal(accountDetails.Email, account.Email);
		Assert.Equal(accountDetails.Username, account.Username);
		Assert.Equal(accountDetails.Bio, account.Bio);
		Assert.Equal(accountDetails.Role, account.Roles[0]);
		Assert.True(account.Enabled);

	}

	[Fact]
	public async Task GetAccountDetails_ShouldReturnCorrectAccount () {

		// Arrange
		using var scope = ServiceScope;
		var accountManager = scope.ServiceProvider.GetRequiredService<AccountManager>();
		var accountDetails = Fakers.Accounts.Generate();
		var createdAccountId = (await accountManager.CreateAccountWithPasswordAsync(accountDetails)).AccountId!.Value;
		var createdAccount = await accountManager.GetAccountDetailsAsync(createdAccountId);

		// Act
		var response = await Client.Authorize(await OwnerJwtToken())
			.GetAsync($"/api/v1/accounts/list/{createdAccountId}/details");

		// Assert
		response.EnsureSuccessStatusCode();
		var account = (await response.Content.ReadFromJsonAsync<AccountProjection>())!;
		Assert.Equal(createdAccountId, account.Id);
		Assert.Equal(createdAccount.Email, account.Email);
		Assert.Equal(createdAccount.Username, account.Username);
		Assert.Equal(createdAccount.Bio, account.Bio);
		Assert.Equal(createdAccount.Roles.Length, account.Roles?.Length);
		Assert.Equal(createdAccount.Roles[0], account.Roles?[0]);
		Assert.True(account.Enabled);
	}

	[Fact]
	public async Task EditAccountDetails_ShouldReturnCorrectAccount () {

		// Arrange
		var scope = ServiceScope;
		var accountManager = scope.ServiceProvider.GetRequiredService<AccountManager>();
		var accountDetails = Fakers.Accounts.Generate();
		accountDetails.Role = Role.Owner.RoleName;
		var createdAccount = await accountManager.CreateAccountWithPasswordAsync(accountDetails);

		var newAccountDetails = Fakers.Accounts.Generate();
		var editRequest = new AccountChangeDetailsOwnerRequestModel {
			Email = newAccountDetails.Email,
			Username = newAccountDetails.Username,
			Bio = newAccountDetails.Bio,
			Enabled = false,
			Roles = new[] { Role.Client.RoleName }
		};

		// Act
		var response = await Client.Authorize(await OwnerJwtToken())
			.PutAsJsonAsync($"/api/v1/accounts/list/{createdAccount.AccountId}/edit", editRequest);

		// Assert
		response.EnsureSuccessStatusCode();
		var account = (await response.Content.ReadFromJsonAsync<AccountProjection>())!;
		Assert.Equal(createdAccount.AccountId, account.Id);
		Assert.Equal(editRequest.Email, account.Email);
		Assert.Equal(editRequest.Username, account.Username);
		Assert.Equal(editRequest.Bio, account.Bio);
		Assert.False(account.Enabled);
		Assert.Equal(editRequest.Roles.Length, account.Roles?.Length);
		Assert.Equal(editRequest.Roles[0], account.Roles?[0]);
		scope.Dispose();

		// Verify that the changes are persisted

		scope = ServiceScope;
		accountManager = scope.ServiceProvider.GetRequiredService<AccountManager>();
		var updatedAccount = await accountManager.GetAccountDetailsAsync(createdAccount.AccountId!.Value);
		Assert.Equal(editRequest.Email, updatedAccount.Email);
		Assert.Equal(editRequest.Username, updatedAccount.Username);
		Assert.Equal(editRequest.Bio, updatedAccount.Bio);
		Assert.False(updatedAccount.Enabled);
		Assert.Equal(editRequest.Roles.Length, updatedAccount.Roles.Length);
		Assert.Equal(editRequest.Roles[0], updatedAccount.Roles[0]);
		scope.Dispose();
	}

}