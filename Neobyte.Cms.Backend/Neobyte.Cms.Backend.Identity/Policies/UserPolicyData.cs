using System.Collections.Generic;

namespace Neobyte.Cms.Backend.Identity.Policies;

public static class UserPolicyData {

	private static readonly Dictionary<UserPolicy, string> PolicyStrings = new Dictionary<UserPolicy, string> {
		{ UserPolicy.OwnerPrivilege, "AdminPrivilege" },
		{ UserPolicy.ClientPrivilege, "ClientPrivilege" }
	};


	private static readonly Dictionary<UserPolicy, UserRole[]> PolicyRoles = new Dictionary<UserPolicy, UserRole[]> {
		{ UserPolicy.OwnerPrivilege, new UserRole[] { UserRole.Owner } },
		{ UserPolicy.ClientPrivilege, new UserRole[] { UserRole.Owner, UserRole.Client } }

	};

	private static readonly UserPolicy[] Policies = new UserPolicy[] {
		UserPolicy.OwnerPrivilege,
		UserPolicy.ClientPrivilege
	};

	public static string GetPolicyName (this UserPolicy policy) {
		return PolicyStrings[policy];
	}

	public static UserRole[] GetPolicyRoles (this UserPolicy policy) {
		return PolicyRoles[policy];
	}

	public static UserPolicy[] GetValues () {
		return Policies;
	}

}