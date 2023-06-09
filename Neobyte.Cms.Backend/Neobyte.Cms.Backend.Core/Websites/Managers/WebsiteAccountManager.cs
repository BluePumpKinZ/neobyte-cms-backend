﻿using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Domain.Websites;
using System;
using System.Linq;

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

	public async Task<WebsiteAccount> AddWebsiteAccountAsync (WebsiteId websiteId, AccountId accountId) {
		var website = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId)
			?? throw new WebsiteNotFoundException($"Website {websiteId} not found");
		var account = await _readOnlyAccountRepository.ReadAccountByIdAsync(accountId)
			?? throw new AccountNotFoundException($"Account {accountId} not found");

		if (account.Roles.Contains(Role.Owner.RoleName))
			throw new AccountIsAdministratorException($"Account {accountId} is an administrator and is already assigned to all websites");

		var wa = await _readOnlyWebsiteAccountRepository.ReadWebsiteAccountByWebsiteIdAndAccountIdAsync(
			websiteId, accountId);
		if (wa != null)
			throw new WebsiteAccountAlreadyExistsException(
				$"WebsiteAccount for website {websiteId} and user {accountId} already exists");

		var websiteAccount = new WebsiteAccount(DateTime.Now) {
			Website = website,
			Account = account
		};
		return await _writeOnlyWebsiteAccountRepository.CreateWebsiteAccountAsync(websiteAccount);
	}

	public async Task DeleteWebsiteAccountAsync (WebsiteId websiteId, AccountId accountId) {
		var websiteAccount = await _readOnlyWebsiteAccountRepository
				.ReadWebsiteAccountByWebsiteIdAndAccountIdAsync(websiteId, accountId)
				?? throw new WebsiteAccountNotFoundException(
				$"WebsiteAccount for website {websiteId} and user {accountId} not found");
		await _writeOnlyWebsiteAccountRepository.DeleteWebsiteAccountAsync(websiteAccount);
	}

	public async Task<IEnumerable<Account>> GetAccountsByWebsiteIdAsync (WebsiteId websiteId) {
		_ = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId)
			?? throw new WebsiteNotFoundException($"Website {websiteId} not found");

		return await _readOnlyWebsiteAccountRepository.ReadAccountsByWebsiteIdAsync(websiteId);
	}

	public async Task<IEnumerable<Website>> GetWebsitesByAccountIdAsync (AccountId accountId) {
		_ = await _readOnlyAccountRepository.ReadAccountByIdAsync(accountId)
			?? throw new AccountNotFoundException($"Account {accountId} not found");

		return await _readOnlyWebsiteAccountRepository.ReadWebsitesByAccountIdAsync(accountId);
	}

	public async Task<IEnumerable<Account>> GetUnassignedAccountsByWebsiteIdAsync (WebsiteId websiteId) {
		_ = await _readOnlyWebsiteRepository.ReadWebsiteByIdAsync(websiteId)
			?? throw new WebsiteNotFoundException($"Website {websiteId} not found");
		
		return await _readOnlyWebsiteAccountRepository.ReadUnassignedAccountsByWebsiteIdAsync(websiteId);
	}

	public async Task<IEnumerable<Website>> GetUnassignedWebsitesAsync (AccountId accountId) {
		var account = await _readOnlyAccountRepository.ReadAccountByIdAsync(accountId)
			?? throw new AccountNotFoundException($"Account {accountId} not found");
		
		if (account.Roles.Select(r => r).Contains(Role.Owner.RoleName))
			return Enumerable.Empty<Website>();
		
		return await _readOnlyWebsiteAccountRepository.ReadUnassignedWebsitesByAccountIdAsync(accountId);
	}
}