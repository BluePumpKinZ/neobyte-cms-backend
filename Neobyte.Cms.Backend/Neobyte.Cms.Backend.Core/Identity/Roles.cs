﻿namespace Neobyte.Cms.Backend.Core.Identity; 

public readonly struct Roles {

	public static Roles Owner { get; } = new Roles("Owner");
	public static Roles Client { get; } = new Roles("Client");

	public string RoleName { get; }

	private Roles (string roleName) {
		RoleName = roleName;
	}

}