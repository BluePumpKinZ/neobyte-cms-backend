using Microsoft.AspNetCore.Identity;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Repositories;
using Neobyte.Cms.Backend.Persistence.Entities.Accounts;
using System.Linq;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Identity.Adapters; 

public class IdentityRoleProvider : IIdentityRoleProvider {

	private readonly UserManager<IdentityAccountEntity> _userManager;
	private readonly IdentityAccountRepository _identityAccountRepository;

	public IdentityRoleProvider (UserManager<IdentityAccountEntity> userManager, IdentityAccountRepository identityAccountRepository) {
		_userManager = userManager;
		_identityAccountRepository = identityAccountRepository;
	}

	public async Task UpdateRoles (Account account) {
		var identityAccount = await _identityAccountRepository.ReadIdentityAccountByAccountIdAsync(account.Id);
		var existingRoles = await _userManager.GetRolesAsync(identityAccount);

		// remove roles
		foreach (var role in existingRoles) {
			if (account.Roles.Contains(role))
				continue;

			await _userManager.RemoveFromRoleAsync(identityAccount, role);
		}

		// add roles
		foreach (var role in account.Roles) {
			if (existingRoles.Contains(role))
				continue;

			await _userManager.AddToRoleAsync(identityAccount, role);
		}
	}

}