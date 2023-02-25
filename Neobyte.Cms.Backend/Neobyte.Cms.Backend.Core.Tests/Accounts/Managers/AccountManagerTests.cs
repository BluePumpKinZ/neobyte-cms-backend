using System;
using System.Linq;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Tests.Accounts.Managers;

public class AccountManagerTests {

	private readonly Mock<IReadOnlyAccountRepository> _readOnlyAccountRepository = new();
	private readonly Mock<IWriteOnlyAccountRepository> _writeOnlyAccountRepository = new();
	private readonly Mock<IIdentityAuthenticationProvider> _identityAuthenticationProvider = new();
	private readonly AccountManager _accountManager;

	public AccountManagerTests () {
		_accountManager = new AccountManager(
			_readOnlyAccountRepository.Object,
			_writeOnlyAccountRepository.Object,
			_identityAuthenticationProvider.Object);
	}

	[Fact]
	public async Task CreateAccountAsync_ShouldCreateNewAccount () {
		// Arrange
		var request = new AccountsCreateRequestModel {
			Email = "test@example.com",
			Username = "testuser",
			Bio = "test bio",
			Role = "Owner",
			Password = "testpass"
		};

		_ = Role.All.SingleOrDefault(r => string.Equals(r.RoleName, request.Role, StringComparison.InvariantCultureIgnoreCase));
		_identityAuthenticationProvider.Setup(x => x.CreateIdentityAccountAsync(It.IsAny<Account>(), It.IsAny<string>())).ReturnsAsync(new AccountsCreateResponseModel(true));

		// Act
		var result = await _accountManager.CreateAccountAsync(request);

		// Assert
		Assert.True(result.Success);
	}

	[Fact]
	public async Task CreateAccountAsync_ShouldReturnErrorForInvalidRole () {
		// Arrange
		var request = new AccountsCreateRequestModel {
			Email = "test@example.com",
			Username = "testuser",
			Bio = "test bio",
			Role = "InvalidRole",
			Password = "testpass"
		};

		_identityAuthenticationProvider.Setup(x => x.CreateIdentityAccountAsync(It.IsAny<Account>(), It.IsAny<string>())).ReturnsAsync(new AccountsCreateResponseModel(true));

		// Act
		var result = await _accountManager.CreateAccountAsync(request);

		// Assert
		Assert.False(result.Success);
		Assert.Contains("does not exist", result.Errors![0]);
	}

	[Fact]
	public async Task GetAccountDetails_ShouldReturnAccountIfExists () {
		// Arrange
		var accountId = new AccountId();
		var account = new Account("test@example.com", "testuser", "test bio", new string[] { "Admin" });
		_readOnlyAccountRepository.Setup(x => x.ReadAccountByIdAsync(accountId)).ReturnsAsync(account);

		// Act
		var result = await _accountManager.GetAccountDetailsAsync(accountId);

		// Assert
		Assert.Equal(account, result);
	}

	[Fact]
	public async Task GetAccountDetails_ShouldThrowExceptionIfAccountNotFound () {
		// Arrange
		var accountId = new AccountId();
		_readOnlyAccountRepository.Setup(x => x.ReadAccountByIdAsync(accountId)).ReturnsAsync((Account?)null);

		// Act & Assert
		await Assert.ThrowsAsync<AccountNotFoundException>(() => _accountManager.GetAccountDetailsAsync(accountId));
	}

	[Fact]
	public async Task GetIdentityAccountWithAccountByEmail_ShouldReturnAccountIfExists () {
		// Arrange
		var email = "test@example.com";
		var account = new Account(email, "testuser", "test bio", new string[] { "Admin" });
		_readOnlyAccountRepository.Setup(x => x.ReadAccountByEmailAsync(email)).ReturnsAsync(account);

		// Act
		var result = await _accountManager.GetAccountWithAccountByEmailAsync(email);

		// Assert
		Assert.Equal(account, result);
	}
}
