namespace Neobyte.Cms.Backend.Core.Mailing.Models; 

public class MailingSendEmailRequestModel {

	public string To { get; set; }
	public string Subject { get; set; }
	public string Body { get; set; }

	public MailingSendEmailRequestModel (string to, string subject, string body) {
		To = to;
		Subject = subject;
		Body = body;
	}

}