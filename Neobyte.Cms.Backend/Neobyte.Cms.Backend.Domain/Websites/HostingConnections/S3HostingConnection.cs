namespace Neobyte.Cms.Backend.Domain.Websites.HostingConnections; 

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct S3HostingConnectionId { }

public class S3HostingConnection : HostingConnection {
	public new S3HostingConnectionId Id { get => new(base.Id.Value); set => base.Id = new HostingConnectionId(value.Value); }
	
	public string Region { get; set; }
	public string BucketName { get; set; }
	public string AccessKey { get; set; }
	public string SecretKey { get; set; }
	
	public S3HostingConnection (S3HostingConnectionId id, string region, string bucketName, string accessKey, string secretKey)
		: base(new HostingConnectionId(id.Value)) {
		Region = region;
		BucketName = bucketName;
		AccessKey = accessKey;
		SecretKey = secretKey;
	}
	
	public S3HostingConnection (string region, string bucketName, string accessKey, string secretKey) {
		Region = region;
		BucketName = bucketName;
		AccessKey = accessKey;
		SecretKey = secretKey;
	}
	
	public override bool Equals (object? obj) {
		return obj is S3HostingConnection connection &&
			   Region == connection.Region &&
			   BucketName == connection.BucketName &&
			   AccessKey == connection.AccessKey &&
			   SecretKey == connection.SecretKey;
	}

	public override int GetHashCode () {
		// ReSharper disable NonReadonlyMemberInGetHashCode
		return HashCode.Combine(Region, BucketName, AccessKey, SecretKey);
	}
}