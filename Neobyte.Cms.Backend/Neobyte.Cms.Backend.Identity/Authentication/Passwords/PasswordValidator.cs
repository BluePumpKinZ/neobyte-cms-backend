using System.Collections.Generic;

namespace Neobyte.Cms.Backend.Identity.Authentication.Passwords;

public class PasswordValidator {

	private readonly IEnumerable<IPasswordRule> _rules;

	public PasswordValidator (IEnumerable<IPasswordRule> rules) {
		_rules = rules;
	}

	public (bool valid, string[] errors) Validate (string password) {
		var errors = new List<string>();

		foreach (var rule in _rules) {
			if (!rule.Applies() || rule.Validate(password, out var error))
				continue;

			errors.Add(error!);
		}
		return (errors.Count == 0, errors.ToArray());

	}

}