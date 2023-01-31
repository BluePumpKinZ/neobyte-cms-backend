using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Identity.Configuration;
using System.Linq;

namespace Neobyte.Cms.Backend.Identity.Authentication.Passwords; 

public class RequireLowercasePasswordRule : IPasswordRule {

	private readonly PasswordOptions _options;

	public RequireLowercasePasswordRule (IOptions<PasswordOptions> options) {
		_options = options.Value;
	}

	public bool Applies () {
		return _options.RequireLowercase;
	}

	public bool Validate (string password, out string? error) {
		bool valid = password.Any(char.IsLower);
		if (!valid) {
			error = "Password must contain at least one lowercase letter.";
			return false;
		}
		error = null;
		return true;
	}

}