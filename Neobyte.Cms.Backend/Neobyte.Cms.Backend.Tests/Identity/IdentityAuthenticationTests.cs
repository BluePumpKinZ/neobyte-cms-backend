using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Identity;
using System.Net;

namespace Neobyte.Cms.Backend.Tests.Identity;

public class IdentityAuthenticationTests : IntegrationTests {

	[Fact]
	public async Task Login_ShouldReturnJwtToken_IfCredentialsAreValid () {

		// Arrange
		using var scope = ServiceScope;
		var accountManager = scope.ServiceProvider.GetRequiredService<AccountManager>();
		await accountManager.CreateAccountAsync(new AccountsCreateRequestModel {
			Username = "Test User",
			Bio = "I am a test user!",
			Email = "test@user.com",
			Password = "Pas$wOrd123",
			Role = Role.Client.RoleName
		});

		// Act
		var response = await Client.PostAsJsonAsync("/api/v1/identity/authentication/login", new {
			Email = "test@user.com",
			Password = "Pas$wOrd123",
			RememberMe = false
		});

		// Assert
		response.EnsureSuccessStatusCode();
		Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType!.ToString());

		var r = new { Token = "", Expires = 0L };

		dynamic? result = response.Content.ReadFromJsonAsync(r.GetType()).Result;
		Assert.NotNull(result);
		Assert.NotNull(result!.Token);
		Assert.NotNull(result.Expires);

	}

	[Fact]
	public async Task Login_ShouldNotWork_IfPasswordIsWrong () {

		// Arrange
		using var scope = ServiceScope;
		var accountManager = scope.ServiceProvider.GetRequiredService<AccountManager>();
		await accountManager.CreateAccountAsync(new AccountsCreateRequestModel {
			Username = "Test User",
			Bio = "I am a test user!",
			Email = "test@user.com",
			Password = "Pas$wOrd123",
			Role = Role.Client.RoleName
		});

		// Act
		var response = await Client.PostAsJsonAsync("/api/v1/identity/authentication/login", new {
			Email = "test@user.com",
			Password = "Wrongpassword",
			RememberMe = false
		});

		// Assert
		Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

	}

	[Fact]
	public async Task Login_ShouldNotWork_IfEmailDoesNotExist () {

		// Act
		var response = await Client.PostAsJsonAsync("/api/v1/identity/authentication/login", new {
			Email = "nonexistent@user.com",
			Password = "Pas$wOrd123",
			RememberMe = false
		});

		// Assert
		Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

	}


}