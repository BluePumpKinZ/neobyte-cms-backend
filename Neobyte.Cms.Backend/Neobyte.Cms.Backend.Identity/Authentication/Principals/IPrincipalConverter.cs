using Microsoft.IdentityModel.Tokens;

namespace Neobyte.Cms.Backend.Identity.Authentication.Principals;

public interface IPrincipalConverter<in TUser, TId, TPrincipal> where TPrincipal : IPrincipal<TId> {

	public TPrincipal FromUser (TUser user);

	public (bool valid, TPrincipal? principal) FromTokenValidationResult (TokenValidationResult tokenValidationResult);

}