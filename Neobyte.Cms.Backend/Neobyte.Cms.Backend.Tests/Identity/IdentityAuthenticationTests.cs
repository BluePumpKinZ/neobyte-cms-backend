namespace Neobyte.Cms.Backend.Tests.Identity;

public class IdentityAuthenticationTests {

	private readonly WebApplicationFactory<Program> _factory;

	public IdentityAuthenticationTests () {
		_factory = new WebApplicationFactory<Program>();
	}

	[Fact]
	public async Task Login_ShouldReturnJwtToken_IfCredentialsAreValid () {

		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/v1/identity/authentication/login", new {
			Email = "admin@neobyte.net",
			Password = "Ne0byteCMS!",
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

}