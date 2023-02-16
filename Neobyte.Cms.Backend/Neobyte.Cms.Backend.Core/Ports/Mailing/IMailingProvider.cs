namespace Neobyte.Cms.Backend.Core.Ports.Mailing; 

public interface IMailingProvider {

	public Task SendMailAsync (string to, string subject, string body);

}