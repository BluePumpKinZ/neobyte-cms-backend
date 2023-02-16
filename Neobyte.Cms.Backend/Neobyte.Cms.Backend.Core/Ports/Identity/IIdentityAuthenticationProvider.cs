using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Core.Ports.Identity;

public interface IIdentityAuthenticationProvider {

	public Task<AccountsCreateResponseModel> CreateIdentityAccountAsync (Account account, string password);

	public Task<bool> LoginAsync (string email, string password);

	public Task<(string token, long expires)> GenerateJwtTokenAsync (Account account, bool rememberMe);

	public string NormalizeEmail (string email);

	public Task<(bool valid, string[]? errors)> ChangePasswordAsync (AccountId accountId, string oldPassword, string newPassword);

}