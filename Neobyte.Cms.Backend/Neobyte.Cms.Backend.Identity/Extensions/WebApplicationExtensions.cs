using Microsoft.AspNetCore.Builder;

namespace Neobyte.Cms.Backend.Identity.Extensions;

public static class WebApplicationExtensions {

	public static WebApplication UseIdentity (this WebApplication app) {

		return app;
	}

}