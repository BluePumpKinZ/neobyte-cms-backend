namespace Neobyte.Cms.Backend.Core.Websites.Models; 

public class WebsiteCreateRequestModel {

	[Required]
	public string Name { get; set; } = string.Empty;
	[Required]
	[Url]
	public string Domain { get; set; } = string.Empty;
	[Required]
	public string HomeFolder { get; set; } = string.Empty;
	[Required]
	public string UploadFolder { get; set; } = string.Empty;
	[Required]
	public string Protocol { get; set; } = string.Empty;

	// Protocols
	public string Host { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public int Port { get; set; }

	// ReSharper disable InconsistentNaming
	public enum HostingProtocol {

		None, FTP, SFTP, SSH, S3

	}
	
}