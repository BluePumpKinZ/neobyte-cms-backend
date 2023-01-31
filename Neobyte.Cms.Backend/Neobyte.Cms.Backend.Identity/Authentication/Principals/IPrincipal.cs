namespace Neobyte.Cms.Backend.Identity.Authentication.Principals;

public interface IPrincipal<TId> {

	public TId Id { get; set; }
	public string[] Roles { get; set; }

}