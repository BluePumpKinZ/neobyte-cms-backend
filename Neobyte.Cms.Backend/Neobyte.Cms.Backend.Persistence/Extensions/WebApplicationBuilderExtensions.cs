using Microsoft.AspNetCore.Builder;

namespace Neobyte.Cms.Backend.Persistence.Extensions; 

public static class WebApplicationBuilderExtensions {

    public static WebApplicationBuilder AddPersistence (this WebApplicationBuilder builder) {
        return builder;
    }

}