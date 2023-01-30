using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using System;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Identity.Adapters;

internal class IdentityAuthenticationProvider : IIdentityAuthenticationProvider {

	public Task<IdentityRegisterResponseModel> Register (IdentityRegisterRequestModel request) {
		throw new NotImplementedException ();
	}

	public Task<IdentityLoginResponseModel> Login (IdentityLoginRequestModel request) {
		throw new NotImplementedException ();
	}

	public Task Logout () {
		throw new NotImplementedException ();
	}

}