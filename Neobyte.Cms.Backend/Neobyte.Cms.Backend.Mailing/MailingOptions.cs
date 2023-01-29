namespace Neobyte.Cms.Backend.Mailing; 

public class MailingOptions {

	public string SenderAddress { get; set; } = string.Empty;
	public string SenderName { get; set; } = string.Empty;
	public MailingSendGridOptions SendGrid { get; set; } = new MailingSendGridOptions();

	public class MailingSendGridOptions {

		public string ApiKey { get; set; } = string.Empty;

	}

}