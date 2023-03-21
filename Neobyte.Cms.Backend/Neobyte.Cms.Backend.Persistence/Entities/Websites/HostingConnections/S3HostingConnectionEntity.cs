using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections; 

public class S3HostingConnectionEntity : HostingConnectionEntity{
	[Required]
	public string AccessKey { get; set; }
	[Required]
	public string SecretKey { get; set; }
	[Required]
	public string BucketName { get; set; }
	[Required]
	public string Region { get; set; }
	
	public S3HostingConnectionEntity(HostingConnectionId id, string accessKey, string secretKey, string bucketName, string region) : base(id){
		AccessKey = accessKey;
		SecretKey = secretKey;
		BucketName = bucketName;
		Region = region;
	}
}