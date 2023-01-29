using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Core.Ports.Mailing;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Neobyte.Cms.Backend.Mailing.Adapters;

internal class SendGridMailingProvider : IMailingProvider {

	private readonly SendGridClient _client;
	private readonly IOptions<MailingOptions> _options;
	private readonly ILogger<SendGridMailingProvider> _logger;

	public SendGridMailingProvider (SendGridClient client, IOptions<MailingOptions> options, ILogger<SendGridMailingProvider> logger) {
		_client = client;
		_options = options;
		_logger = logger;
	}

	public async Task SendMailAsync (string to, string subject, string body) {

		var fromAddress = new EmailAddress(_options.Value.SenderAddress, _options.Value.SenderName);
		var toAddress = MailHelper.StringToEmailAddress(to);
		var email = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, body, body);
		var response = await _client.SendEmailAsync(email);

		if (response.IsSuccessStatusCode)
			return;

		_logger.LogWarning("Failed to send email to {to}. Response code {code}", to, response.StatusCode);
	}

}