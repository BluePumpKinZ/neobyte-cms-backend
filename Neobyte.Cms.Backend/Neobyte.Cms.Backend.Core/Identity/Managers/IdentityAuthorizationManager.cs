using Microsoft.AspNetCore.Identity;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Identity.Managers; 

public class IdentityAuthorizationManager {

	private readonly IIdentityAuthorizationProvider _identityAuthorizationProvider;

	public IdentityAuthorizationManager (IIdentityAuthorizationProvider identityAuthorizationProvider) {
		_identityAuthorizationProvider = identityAuthorizationProvider; ;
	}

}