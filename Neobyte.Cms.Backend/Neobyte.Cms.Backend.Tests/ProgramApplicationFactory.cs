using Microsoft.AspNetCore.Hosting;

namespace Neobyte.Cms.Backend.Tests;

public class ProgramApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class {

	protected override void ConfigureWebHost (IWebHostBuilder builder) {

		Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
		builder.UseEnvironment("Testing");

	}

}