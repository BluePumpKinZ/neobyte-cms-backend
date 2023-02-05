using Microsoft.AspNetCore.Identity;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using System;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Identity.Adapters; 

public class IdentityRoleProvider : IIdentityRoleProvider {

	private readonly UserManager<IdentityAccount> _userManager;
	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;

	public IdentityRoleProvider (UserManager<IdentityAccount> userManager, IReadOnlyAccountRepository readOnlyAccountRepository) {
		_userManager = userManager;
		_readOnlyAccountRepository = readOnlyAccountRepository;
	}

	public async Task AddRoleToIdentityUserAsync (Guid identityAccountId, Role role) {
		var identityAccount = await _readOnlyAccountRepository.ReadByIdentityAccountIdAsync(identityAccountId);
		await _userManager.AddToRoleAsync(identityAccount, role.RoleName);
	}

}