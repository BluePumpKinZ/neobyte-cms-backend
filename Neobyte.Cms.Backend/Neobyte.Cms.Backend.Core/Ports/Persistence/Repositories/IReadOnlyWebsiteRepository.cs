﻿using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories; 

public interface IReadOnlyWebsiteRepository {

	public Task<Website?> ReadWebsiteByIdAsync (WebsiteId websiteId);

	public Task<IEnumerable<Website>> ReadAllWebsitesAsync ();

}