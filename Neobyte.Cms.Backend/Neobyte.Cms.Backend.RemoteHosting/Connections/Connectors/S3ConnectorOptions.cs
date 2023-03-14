namespace Neobyte.Cms.Backend.RemoteHosting.Connections.Connectors; 

public class S3ConnectorOptions {
	
	public string AccessKey { get; set; } = string.Empty;
	public string SecretKey { get; set; } = string.Empty;
	public string BucketName { get; set; } = string.Empty;
	public string Region { get; set; } = string.Empty;
}