using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Core.Ports.Mailing;
using Neobyte.Cms.Backend.Mailing.Adapters;
using SendGrid;

namespace Neobyte.Cms.Backend.Mailing.Extensions; 

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddMailing (this WebApplicationBuilder builder) {

		builder.Services.Configure<MailingOptions>(builder.Configuration.GetSection("Mailing"));

		builder.Services.AddSingleton(sp => {
			var options = sp.GetRequiredService<IOptions<MailingOptions>>().Value;
			return new SendGridClient(options.SendGrid.ApiKey);
		});
		builder.Services.AddSingleton<IMailingProvider, SendGridMailingProvider>();

		return builder;
	}

}