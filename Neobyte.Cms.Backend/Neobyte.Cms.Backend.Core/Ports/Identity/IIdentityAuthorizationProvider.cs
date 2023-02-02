namespace Neobyte.Cms.Backend.Core.Ports.Identity; 

public interface IIdentityAuthorizationProvider {

	public bool IsAuthorized (string policyName, string[] roles);

}