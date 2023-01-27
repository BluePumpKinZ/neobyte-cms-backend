using Microsoft.AspNetCore.Builder;

namespace Neobyte.Cms.Backend.Monitoring.Extensions; 

public static class WebApplicationBuilderExtensions {

    public static WebApplicationBuilder AddMonitoring (this WebApplicationBuilder builder) {
        return builder;
    }

}