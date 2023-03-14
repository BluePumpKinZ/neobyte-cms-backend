using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Neobyte.Cms.Backend.Core.RemoteHosting;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;
using Neobyte.Cms.Backend.Utils;

namespace Neobyte.Cms.Backend.RemoteHosting.Connections.Connectors;

public class S3Connector : IRemoteHostingConnector {
	private AmazonS3Client? _client;
	private TransferUtility? _transferUtility;
	private readonly S3ConnectorOptions _options = new();
	private readonly PathUtils _pathUtils;

	public DateTime LastConnectionTime { get; set; }

	public S3Connector (PathUtils pathUtils) {
		_pathUtils = pathUtils;
	}

	private AmazonS3Client Client {
		get {
			_client ??= GetClient().Result;
			return _client;
		}
	}

	private TransferUtility TransferUtility {
		get {
			_transferUtility ??= GetTransferUtility().Result;
			return _transferUtility;
		}
	}

	private Task<AmazonS3Client> GetClient () {
		var client = new AmazonS3Client(_options.AccessKey, _options.SecretKey,
			Amazon.RegionEndpoint.GetBySystemName(_options.Region));
		return Task.FromResult(client);
	}

	private Task<TransferUtility> GetTransferUtility () {
		var transferUtility = new TransferUtility(Client);
		return Task.FromResult(transferUtility);
	}

	public bool CanConnect (HostingConnection connection) {
		return connection is S3HostingConnection;
	}

	public void Configure (HostingConnection connection) {
		var s3Connection = (S3HostingConnection)connection;
		_options.AccessKey = s3Connection.AccessKey;
		_options.SecretKey = s3Connection.SecretKey;
		_options.BucketName = s3Connection.BucketName;
		_options.Region = s3Connection.Region;
	}

	public async Task<bool> ValidateAsync () {
		try {
			var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(Client, _options.BucketName);
			return true;
		} catch (Exception ex) when (ex is AmazonS3Exception or TimeoutException) {
			return false;
		}
	}

	public async Task<IEnumerable<FilesystemEntry>> ListItemsAsync (string path) {
		var items = await Client.ListObjectsAsync(new ListObjectsRequest {
			BucketName = _options.BucketName,
			Prefix = path
		});
		return items.S3Objects.Select(i => {
			var meta = Client.GetObjectMetadataAsync(new GetObjectMetadataRequest {
				BucketName = _options.BucketName,
				Key = i.Key
			}).Result;
			return new FilesystemEntry(i.Key, meta.Headers.ContentLength, i.LastModified);
		});
		
	}

	public async Task CreateFolderAsync (string path) {
		await Client.PutObjectAsync(new PutObjectRequest {
			BucketName = _options.BucketName,
			Key = path,
			ContentBody = string.Empty
		});
	}

	public async Task RenameFolderAsync (string path, string newPath) {
		await Client.CopyObjectAsync(new CopyObjectRequest {
			SourceBucket = _options.BucketName,
			SourceKey = path,
			DestinationBucket = _options.BucketName,
			DestinationKey = newPath
		});
		await Client.DeleteObjectAsync(new DeleteObjectRequest {
			BucketName = _options.BucketName,
			Key = path
		});
	}

	public async Task DeleteFolderAsync (string path) {
		await Client.DeleteObjectAsync(new DeleteObjectRequest {
			BucketName = _options.BucketName,
			Key = path
		});
	}

	public async Task CreateFileAsync (string path, byte[] content) {
		await TransferUtility.UploadAsync(new MemoryStream(content), _options.BucketName, path);
	}

	public async Task RenameFileAsync (string path, string newPath) {
		await Client.CopyObjectAsync(new CopyObjectRequest {
			SourceBucket = _options.BucketName,
			SourceKey = path,
			DestinationBucket = _options.BucketName,
			DestinationKey = newPath
		});
		await Client.DeleteObjectAsync(new DeleteObjectRequest {
			BucketName = _options.BucketName,
			Key = path
		});
	}

	public async Task DeleteFileAsync (string path) {
		await Client.DeleteObjectAsync(new DeleteObjectRequest {
			BucketName = _options.BucketName,
			Key = path
		});
	}

	public async Task<byte[]> GetFileContentAsync (string path) {
		var response = await Client.GetObjectAsync(new GetObjectRequest {
			BucketName = _options.BucketName,
			Key = path
		});
		using var responseStream = response.ResponseStream;
		using var memoryStream = new MemoryStream();
		await responseStream.CopyToAsync(memoryStream);
		return memoryStream.ToArray();
	}

	public async Task<bool> FolderExistsAsync (string path) {
		var response = await Client.ListObjectsAsync(new ListObjectsRequest {
			BucketName = _options.BucketName,
			Prefix = path
		});
		return response.S3Objects.Any();
	}

	public async Task<bool> FileExistsAsync (string path) {
		return await FolderExistsAsync(path);
	}

	public async Task<FilesystemEntry> GetFilesystemEntryInfo (string path) {
		var meta = await Client.GetObjectMetadataAsync(new GetObjectMetadataRequest {
			BucketName = _options.BucketName,
			Key = path
		});
		var info = await Client.GetObjectAsync(new GetObjectRequest {
			BucketName = _options.BucketName,
			Key = path
		});
		return new FilesystemEntry(info.Key, meta.Headers.ContentLength, info.LastModified);
	}

	public void Disconnect () {
		_client?.Dispose();
	}
}