using Neobyte.Cms.Backend.Core.Mailing.Models;
using System.Threading.Tasks;
using Neobyte.Cms.Backend.Core.Ports.Mailing;

namespace Neobyte.Cms.Backend.Core.Mailing.Managers; 

public class MailingManager {

	private readonly IMailingProvider _mailingProvider;

	public MailingManager (IMailingProvider mailingProvider) {
		_mailingProvider = mailingProvider;
	}

	public async Task SendEmail (MailingSendEmailRequestModel request) {
		await _mailingProvider.SendMailAsync(request.To, request.Subject, request.Body);
	}

}