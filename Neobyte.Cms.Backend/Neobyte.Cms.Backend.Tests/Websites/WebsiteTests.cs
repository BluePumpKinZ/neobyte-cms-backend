using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;

namespace Neobyte.Cms.Backend.Tests.Websites; 

public class WebsiteTests : IntegrationTests {

	[Fact]
	public async Task CreateWebsite_ShouldWork () {

		var website = Fakers.Websites.Generate();

		var response = await Client.Authorize(await OwnerJwtToken())
			.PostAsJsonAsync("/api/v1/websites/create", website);
		Assert.NotNull(response);
		var content = await response.Content.ReadAsStringAsync();

		using var scope = ServiceScope;
		var websiteRepository = scope.ServiceProvider.GetRequiredService<IReadOnlyWebsiteRepository>();

		var createdWebsite = (await websiteRepository.ReadAllWebsitesAsync()).First();
		Assert.Equal(website.Domain, createdWebsite.Domain);
		Assert.Equal(website.Name, createdWebsite.Name);
		Assert.Equal(website.HomeFolder, createdWebsite.HomeFolder);
		Assert.Equal(website.UploadFolder, createdWebsite.UploadFolder);

	}

}