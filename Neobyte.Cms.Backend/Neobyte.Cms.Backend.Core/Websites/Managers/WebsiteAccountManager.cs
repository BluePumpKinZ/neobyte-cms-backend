using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Core.Websites.Models;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Domain.Websites;
using System;

namespace Neobyte.Cms.Backend.Core.Websites.Managers;

public class WebsiteAccountManager {
	private readonly IReadOnlyWebsiteAccountRepository _readOnlyWebsiteAccountRepository;
	private readonly IWriteOnlyWebsiteAccountRepository _writeOnlyWebsiteAccountRepository;
	private readonly IReadOnlyWebsiteRepository _readOnlyWebsiteRepository;
	private readonly IReadOnlyAccountRepository _readOnlyAccountRepository;

	public WebsiteAccountManager (IReadOnlyWebsiteAccountRepository readOnlyWebsiteAccountRepository,
		IWriteOnlyWebsiteAccountRepository writeOnlyWebsiteAccountRepository,
		IReadOnlyWebsiteRepository readOnlyWebsiteRepository, IReadOnlyAccountRepository readOnlyAccountRepository) {
		_readOnlyWebsiteAccountRepository = readOnlyWebsiteAccountRepository;
		_writeOnlyWebsiteAccountRepository = writeOnlyWebsiteAccountRepository;
		_readOnlyWebsiteRepository = readOnlyWebsiteRepository;
		_readOnlyAccountRepository = readOnlyAccountRepository;
	}

	public async Task<WebsiteAccount> AddWebsiteAccountAsync (WebsiteCreateWebsiteAccountRequestModel request) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(request.WebsiteId);
		if (website is null)
			throw new WebsiteNotFoundException($"Website {request.WebsiteId} not found");
		var account = await _readOnlyAccountRepository.ReadAccountByIdAsync(request.AccountId);
		if (account is null)
			throw new AccountNotFoundException($"Account {request.AccountId} not found");
		
		var wa = await _readOnlyWebsiteAccountRepository.ReadWebsiteAccountByWebsiteIdAndAccountIdAsync(request.WebsiteId, request.AccountId);
		if (wa != null)
			throw new WebsiteAccountAlreadyExistsException($"WebsiteAccount for website {request.WebsiteId} and user {request.AccountId} already exists");
		
		var websiteAccount = new WebsiteAccount(DateTime.Now) {
			Website = website,
			Account = account
		};
		return await _writeOnlyWebsiteAccountRepository.CreateWebsiteAccountAsync(websiteAccount);
	}
	
	public async Task DeleteWebsiteAccountAsync (WebsiteId websiteId, AccountId accountId) {
		
		var websiteAccount = await _readOnlyWebsiteAccountRepository.ReadWebsiteAccountByWebsiteIdAndAccountIdAsync(websiteId, accountId);
		if (websiteAccount is null)
			throw new WebsiteAccountNotFoundException($"WebsiteAccount for website {websiteId} and user {accountId} not found");
		await _writeOnlyWebsiteAccountRepository.DeleteWebsiteAccountAsync(websiteAccount);
	}

	public async Task<IEnumerable<Account>> GetAccountsByWebsiteIdAsync (WebsiteId websiteId) {
		return await _readOnlyWebsiteAccountRepository.ReadAccountsByWebsiteIdAsync(websiteId);
	}

	public async Task<IEnumerable<Website>> GetWebsitesByAccountIdAsync (AccountId accountId) {
		return await _readOnlyWebsiteAccountRepository.ReadWebsitesByAccountIdAsync(accountId);
	}
}