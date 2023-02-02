using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Identity.Managers;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Authorization;
using Neobyte.Cms.Backend.Identity.Authorization.Policies;
using System.Linq;

namespace Neobyte.Cms.Backend.Identity.Extensions;

public static class WebApplicationExtensions {

	public static WebApplication UseIdentity (this WebApplication app) {

		var scope = app.Services.CreateScope();

		// Ensure all roles are created and persisted
		var authorizationManager = scope.ServiceProvider.GetRequiredService<IdentityAuthorizationManager>();
		foreach (var role in Roles.All) {
			if (authorizationManager.GetRoleByName(role.RoleName).Result == null) {
				authorizationManager.AddRole(new Role(role.RoleName)).Wait();
			}
		}

		// Create policies in policy store
		var policyStore = scope.ServiceProvider.GetRequiredService<AuthorizationManager>();
		foreach (var privilege in Privileges.All) {
			policyStore.AddPolicy(privilege.PrivilegeName,
				privilege.PrivilegeRoles.Select(r => new PolicyRole { Name = r.RoleName }).ToArray());
		}

		return app;
	}

}