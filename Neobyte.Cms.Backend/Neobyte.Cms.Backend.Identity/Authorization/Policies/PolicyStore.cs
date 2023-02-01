using System.Collections.Generic;

namespace Neobyte.Cms.Backend.Identity.Authorization.Policies; 

public class PolicyStore {

	public Dictionary<string, PolicyRole[]> Policies { get; } = new();

}