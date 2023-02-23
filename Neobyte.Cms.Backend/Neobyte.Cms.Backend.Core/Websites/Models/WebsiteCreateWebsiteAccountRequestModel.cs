using Neobyte.Cms.Backend.Domain.Accounts;
using Neobyte.Cms.Backend.Domain.Websites;
using System;

namespace Neobyte.Cms.Backend.Core.Websites.Models; 

public class WebsiteCreateWebsiteAccountRequestModel {
	[Required]
	public WebsiteId WebsiteId { get; set; }
	[Required]
	public AccountId AccountId { get; set; }
}