using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Identity.Policies;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Identity.Extensions;

public static class WebApplicationExtensions {

	public static async Task<WebApplication> UseIdentity (this WebApplication app) {

		using var scope = app.Services.CreateScope();

		var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
		
		foreach (UserRole userRole in UserRole.Values) {
			if (await roleManager.RoleExistsAsync(userRole))
				continue;

			IdentityRole role = new(userRole);
			await roleManager.CreateAsync(role);
		}

		app.UseAuthentication();
		app.UseAuthorization();

		return app;
	}

}