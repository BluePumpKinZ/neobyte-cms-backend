using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Identity.Configuration;
using System.Linq;

namespace Neobyte.Cms.Backend.Identity.Authentication.Passwords; 

public class RequireDigitPasswordRule : IPasswordRule {

	private readonly PasswordOptions _options;

	public RequireDigitPasswordRule (IOptions<PasswordOptions> options) {
		_options = options.Value;
	}

	public bool Applies () {
		return _options.RequireDigit;
	}

	public bool Validate (string password, out string? error) {
		bool valid = password.Any(char.IsDigit);
		if (!valid) {
			error = "Password must contain at least one digit.";
			return false;
		}
		error = null;
		return true;
	}

}