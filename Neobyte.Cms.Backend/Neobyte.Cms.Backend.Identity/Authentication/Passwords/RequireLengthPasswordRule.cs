using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Identity.Configuration;

namespace Neobyte.Cms.Backend.Identity.Authentication.Passwords; 

public class RequireLengthPasswordRule : IPasswordRule {

	private readonly PasswordOptions _options;

	public RequireLengthPasswordRule (IOptions<PasswordOptions> options) {
		_options = options.Value;
	}

	public bool Applies () {
		return true;
	}

	public bool Validate (string password, out string? error) {
		bool valid = password.Length >= _options.RequiredLength;
		if (!valid) {
			error = $"Password must be at least {_options.RequiredLength} characters long.";
			return false;
		}
		error = null;
		return true;
	}

}