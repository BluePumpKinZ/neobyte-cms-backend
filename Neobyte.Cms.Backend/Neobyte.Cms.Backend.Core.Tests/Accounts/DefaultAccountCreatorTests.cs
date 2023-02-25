using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Core.Accounts.Managers;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Configuration;
using System.Threading.Tasks;
using Neobyte.Cms.Backend.Core.Accounts;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Ports.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Core.Tests.Accounts;

public class DefaultAccountCreatorTests {

	private readonly DefaultAccountOptions _defaultAccountOptions;
	private readonly DefaultAccountCreator _defaultAccountCreator;
	private readonly Mock<IReadOnlyAccountRepository> _readOnlyAccountRepository = new();
	private readonly Mock<IWriteOnlyAccountRepository> _writeOnlyAccountRepository = new();
	private readonly Mock<IIdentityAuthenticationProvider> _identityAuthenticationProvider = new();

	public DefaultAccountCreatorTests () {
		var mockOptions = new Mock<IOptions<CoreOptions>>();
		var accountManager = new AccountManager(_readOnlyAccountRepository.Object,
			_writeOnlyAccountRepository.Object, _identityAuthenticationProvider.Object);
		var logger = new Mock<ILogger<DefaultAccountCreator>>();
		_defaultAccountOptions = new DefaultAccountOptions {
			AddOnAccountsEmpty = true,
			Username = "testuser",
			Bio = "test bio",
			Email = "testuser@test.com",
			Password = "testpassword"
		};
		mockOptions.Setup(x => x.Value).Returns(new CoreOptions { DefaultAccount = _defaultAccountOptions });
		_defaultAccountCreator = new DefaultAccountCreator(mockOptions.Object, accountManager, logger.Object);
	}

	[Fact]
	public async Task CreateDefaultAccount_ShouldNotCreateAccount_IfAddOnAccountsEmptyIsFalse () {
		// Arrange
		_defaultAccountOptions.AddOnAccountsEmpty = false;

		// Act
		await _defaultAccountCreator.CreateDefaultAccount();

		// Assert
		_readOnlyAccountRepository.Verify(x => x.ReadOwnerAccountExistsAsync(), Times.Never);
		_identityAuthenticationProvider.Verify(x => x.CreateIdentityAccountAsync(It.IsAny<Account>(), It.IsAny<string>()), Times.Never);
	}

	[Fact]
	public async Task CreateDefaultAccount_ShouldNotCreateAccount_IfOwnerAccountAlreadyExists () {
		// Arrange
		_readOnlyAccountRepository.Setup(x => x.ReadOwnerAccountExistsAsync()).ReturnsAsync(true);

		// Act
		await _defaultAccountCreator.CreateDefaultAccount();

		// Assert
		_readOnlyAccountRepository.Verify(x => x.ReadOwnerAccountExistsAsync(), Times.Once);
		_identityAuthenticationProvider.Verify(x => x.CreateIdentityAccountAsync(It.IsAny<Account>(), It.IsAny<string>()), Times.Never);
	}

	[Fact]
	public async Task CreateDefaultAccount_ShouldCreateAccount_IfAddOnAccountsEmptyIsTrueAndOwnerAccountDoesNotExist () {
		// Arrange
		_readOnlyAccountRepository.Setup(x => x.ReadOwnerAccountExistsAsync()).ReturnsAsync(false);
		_identityAuthenticationProvider.Setup(x => x.CreateIdentityAccountAsync(It.IsAny<Account>(), It.IsAny<string>()))
			.ReturnsAsync(new AccountsCreateResponseModel(true));

		// Act
		await _defaultAccountCreator.CreateDefaultAccount();

		// Assert
		_readOnlyAccountRepository.Verify(x => x.ReadOwnerAccountExistsAsync(), Times.Once);
		_identityAuthenticationProvider.Verify(x => x.CreateIdentityAccountAsync(It.Is<Account>(r =>
			r.Email == _defaultAccountOptions.Email &&
			r.Username == _defaultAccountOptions.Username &&
			r.Bio == _defaultAccountOptions.Bio &&
			r.Roles.Length == 1 &&
			r.Roles[0] == Role.Owner.RoleName),
			_defaultAccountOptions.Password), Times.Once);
	}

}