using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Core.Identity.Models.Authentication;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Identity.Authentication.Passwords;
using Neobyte.Cms.Backend.Identity.Configuration;
using System.Threading.Tasks;
using PasswordHasher = Microsoft.AspNetCore.Identity.PasswordHasher<Neobyte.Cms.Backend.Domain.Accounts.Account>;

namespace Neobyte.Cms.Backend.Identity.Authentication;

public class AuthenticationManager {

	private readonly PasswordHasher _passwordHasher;
	private readonly PasswordValidator _passwordValidator;
	private readonly IdentityOptions _options;
	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;
	private readonly IWriteOnlyAccountRepository _writeOnlyAccountRepository;

	public AuthenticationManager (PasswordHasher passwordHasher, IOptions<IdentityOptions> options, IReadOnlyAccountRepository readOnlyAccountRepository, IWriteOnlyAccountRepository writeOnlyAccountRepository, PasswordValidator passwordValidator) {
		_passwordHasher = passwordHasher;
		_options = options.Value;
		_readOnlyAccountRepository = readOnlyAccountRepository;
		_writeOnlyAccountRepository = writeOnlyAccountRepository;
		_passwordValidator = passwordValidator;
	}

	public async Task<(RegisterResult result, string[]? errors)> RegisterAsync (IdentityRegisterRequestModel request) {
		var existingAccount = await _readOnlyAccountRepository.ReadAccountByEmailAsync(request.Email);
		if (existingAccount != null)
			return (RegisterResult.EmailAlreadyExists, new string[] { "Email already exists" });

		var passwordValidationResult = _passwordValidator.Validate(request.Password);
		if (!passwordValidationResult.valid)
			return (RegisterResult.InvalidPassword, passwordValidationResult.errors);

		var account = new Account(request.Email, request.FirstName, request.LastName);
		await EncodePasswordAsync(account, request.Password);
		await _writeOnlyAccountRepository.CreateAccountAsync(account);

		return (RegisterResult.Success, null);
	}

	public async Task<LoginResult> LoginAsync (IdentityLoginRequestModel request) {
		var account = await _readOnlyAccountRepository.ReadAccountByEmailAsync(request.Email);
		if (account is null)
			return LoginResult.InvalidCredentials;

		if (!CanSignIn(account))
			return LoginResult.NotAllowed;

		var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(account, account.EncodedPassword, request.Password);
		if (passwordVerificationResult == Microsoft.AspNetCore.Identity.PasswordVerificationResult.SuccessRehashNeeded)
			await EncodePasswordAsync(account, request.Password, true);

		if (passwordVerificationResult != Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
			return LoginResult.InvalidCredentials;

		return LoginResult.Success;
	}

	public bool CanSignIn (Account account) {
		if (!account.Enabled)
			return false;

		if (!account.EmailConfirmed && _options.SignIn.RequireConfirmedEmail)
			return false;

		return true;
	}

	public async Task<(bool success, string[]? errors)> EncodePasswordAsync (Account account, string password, bool update = false) {
		(bool valid, string[]? errors) = _passwordValidator.Validate(password);
		if (!valid)
			return (false, errors);

		var hashedPassword = _passwordHasher.HashPassword(account, password);
		account.EncodedPassword = hashedPassword;
		if (update)
			await _writeOnlyAccountRepository.UpdateAccountAsync(account);
		return (true, null);
	}

	

}