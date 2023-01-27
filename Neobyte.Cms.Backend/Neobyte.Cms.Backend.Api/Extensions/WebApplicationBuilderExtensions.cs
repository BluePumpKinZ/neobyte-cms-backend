using Microsoft.AspNetCore.Builder;

namespace Neobyte.Cms.Backend.Api.Extensions; 

public static class WebApplicationBuilderExtensions {

    public static WebApplicationBuilder AddApi (this WebApplicationBuilder builder) {
        return builder;
    }

}