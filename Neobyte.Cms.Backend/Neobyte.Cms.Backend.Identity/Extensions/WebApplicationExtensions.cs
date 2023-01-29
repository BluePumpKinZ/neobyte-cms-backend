using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Identity.Policies;

namespace Neobyte.Cms.Backend.Identity.Extensions;

public static class WebApplicationExtensions {

	public static WebApplication UseIdentity (this WebApplication app) {

		using var scope = app.Services.CreateScope();

		var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
		
		foreach (UserRole userRole in UserRole.Values) {
			if (roleManager.RoleExistsAsync(userRole).Result)
				continue;

			IdentityRole role = new(userRole);
			roleManager.CreateAsync(role).Wait();
		}

		app.UseAuthentication();
		app.UseAuthorization();

		return app;
	}

}