using Microsoft.AspNetCore.Identity;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Identity.Managers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Identity.Initializers;

public class RoleInitializer {

	private readonly RoleManager<IdentityRole<Guid>> _roleManager;

	public RoleInitializer (RoleManager<IdentityRole<Guid>> roleManager) {
		_roleManager = roleManager;
	}

	public async Task InitializeRoles () {
		foreach (var role in Role.All) {
			if (await _roleManager.RoleExistsAsync(role.RoleName))
				continue;

			var identityRole = new IdentityRole<Guid>(role.RoleName) { Id = Guid.NewGuid() };
			await _roleManager.CreateAsync(identityRole);
		}
	}

}