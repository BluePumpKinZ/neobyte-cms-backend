using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Neobyte.Cms.Backend.Utils.Extensions; 

public static class WebApplicationBuilderExtensions {

    public static WebApplicationBuilder AddUtils (this WebApplicationBuilder builder) {
		builder.Services.AddSingleton<PathUtils>();
		builder.Services.AddSingleton<TypeUtils>();
		return builder;
    }

}