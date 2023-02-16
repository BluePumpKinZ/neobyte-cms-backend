using System;

namespace Neobyte.Cms.Backend.Core.Websites.Models;

public class WebsiteEditRequestModel : WebsiteCreateRequestModel {

	[Required]
	public Guid Id { get; set; }

}