using Microsoft.AspNetCore.Hosting;

namespace Neobyte.Cms.Backend.Tests;

internal class ProgramApplicationFactory : WebApplicationFactory<Program> {

	protected override void ConfigureWebHost (IWebHostBuilder builder) {

		Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
		builder.UseEnvironment("Testing");

	}

}