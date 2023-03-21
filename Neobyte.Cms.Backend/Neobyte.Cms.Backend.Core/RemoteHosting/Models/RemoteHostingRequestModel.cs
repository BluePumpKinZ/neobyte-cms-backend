namespace Neobyte.Cms.Backend.Core.RemoteHosting.Models; 

public class RemoteHostingRequestModel {

	[Required]
	public string Protocol { get; set; } = string.Empty;
	public string Host { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public int Port { get; set; }
	public string Region { get; set; } = string.Empty;
	public string BucketName { get; set; } = string.Empty;
	public string AccessKey { get; set; } = string.Empty;
	public string SecretKey { get; set; } = string.Empty;

	// ReSharper disable InconsistentNaming
	public enum HostingProtocol {

		FTP, SFTP, S3

	}

}