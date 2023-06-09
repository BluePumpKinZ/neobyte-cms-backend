﻿using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Core.Accounts.Models; 

public class AccountsCreateResponseModel {

	public bool Success { get; set; }
	public AccountId? AccountId { get; set; }
	public string[]? Errors { get; set; }

	public AccountsCreateResponseModel (bool success) {
		Success = success;
	}

}