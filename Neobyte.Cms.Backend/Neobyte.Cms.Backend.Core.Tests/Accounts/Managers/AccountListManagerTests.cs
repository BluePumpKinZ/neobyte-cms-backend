using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Core.Tests.Accounts.Managers;

public class AccountListManagerTests {

	private readonly AccountListManager _accountListManager;
	private readonly Mock<IReadOnlyAccountRepository> _readOnlyAccountRepository = new();
	private readonly Mock<IWriteOnlyAccountRepository> _writeOnlyAccountRepository = new();
	private readonly Mock<IIdentityRoleProvider> _identityRoleProvider = new();

	public AccountListManagerTests () {
		_accountListManager = new AccountListManager(
			_readOnlyAccountRepository.Object,
			_writeOnlyAccountRepository.Object,
			_identityRoleProvider.Object);
	}

	[Fact]
	public async Task EditAccountDetailsAsync_ShouldUpdateAccountDetails () {
		// Arrange
		var account = new Account(
			"john.doe@example.com",
			"johndoe",
			"Hello, world!", new string[] { "Client" });
		var request = new AccountChangeDetailsOwnerRequestModel {
			AccountId = account.Id,
			Email = "jane.doe@example.com",
			Username = "janedoe",
			Bio = "Hi, there!",
			Enabled = false,
			Roles = new string[] { "Client", "Owner" }
		};
		_readOnlyAccountRepository.Setup(x => x.ReadAccountByIdAsync(account.Id))
			.ReturnsAsync(account);
		_writeOnlyAccountRepository.Setup(x => x.UpdateAccountAsync(account))
			.ReturnsAsync(account);

		// Act
		var result = await _accountListManager.EditAccountDetailsAsync(request);

		// Assert
		Assert.Equal(request.Email, result.Email);
		Assert.Equal(request.Username, result.Username);
		Assert.Equal(request.Bio, result.Bio);
		Assert.Equal(request.Enabled, result.Enabled);
		Assert.Equal(request.Roles, result.Roles);
		_writeOnlyAccountRepository.Verify(x => x.UpdateAccountAsync(account), Times.Once);
		_identityRoleProvider.Verify(x => x.UpdateRoles(account), Times.Once);
	}

	[Fact]
	public async Task EditAccountDetailsAsync_ShouldThrowAccountNotFoundException_WhenAccountIsNull () {
		// Arrange
		var request = new AccountChangeDetailsOwnerRequestModel { AccountId = AccountId.New() };
		_readOnlyAccountRepository.Setup(x => x.ReadAccountByIdAsync(request.AccountId))
			.ReturnsAsync((Account?)null);

		// Act and Assert
		await Assert.ThrowsAsync<AccountNotFoundException>(() =>
			_accountListManager.EditAccountDetailsAsync(request));
	}
}