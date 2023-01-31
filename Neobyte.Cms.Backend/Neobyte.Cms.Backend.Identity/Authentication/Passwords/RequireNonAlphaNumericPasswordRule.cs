using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Identity.Configuration;
using System.Linq;

namespace Neobyte.Cms.Backend.Identity.Authentication.Passwords; 

public class RequireNonAlphaNumericPasswordRule : IPasswordRule {

	private readonly PasswordOptions _options;

	public RequireNonAlphaNumericPasswordRule (IOptions<PasswordOptions> options) {
		_options = options.Value;
	}

	public bool Applies () {
		return _options.RequireNonAlphanumeric;
	}

	public bool Validate (string password, out string? error) {
		bool valid = password.Any(c => !char.IsLetterOrDigit(c));
		if (!valid) {
			error = "Password must contain at least one uppercase letter.";
			return false;
		}
		error = null;
		return true;
	}

}