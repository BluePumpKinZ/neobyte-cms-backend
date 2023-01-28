using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Ports.Identity; 

public interface IIdentityAuthenticationProvider {

	public Task<IdentityRegisterResponseModel> Register (IdentityRegisterRequestModel request);

	public Task<IdentityLoginResponseModel> Login (IdentityLoginRequestModel request);

	public Task Logout ();

}