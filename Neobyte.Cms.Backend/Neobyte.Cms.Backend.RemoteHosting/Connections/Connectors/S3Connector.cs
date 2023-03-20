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

	public DateTime LastConnectionTime { get; set; } = DateTime.UtcNow;

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
			await AmazonS3Util.DoesS3BucketExistV2Async(Client, _options.BucketName);
			return true;
		} catch (Exception ex) when (ex is AmazonS3Exception or TimeoutException) {
			return false;
		}
	}

	public async Task<IEnumerable<FilesystemEntry>> ListItemsAsync (string path) {
		path = _pathUtils.GetS3Path(path);
		var items = await Client.ListObjectsAsync(new ListObjectsRequest {
			BucketName = _options.BucketName,
			Prefix = path
		});
		return await GetFilesystemEntriesByS3Objects(items.S3Objects, path);

	}

	public async Task CreateFolderAsync (string path) {
		path = _pathUtils.GetS3Path(path);
		await Client.PutObjectAsync(new PutObjectRequest {
			BucketName = _options.BucketName,
			Key = path + "/",
			ContentBody = string.Empty
		});
	}

	public async Task RenameFolderAsync (string path, string newPath) {
		path = _pathUtils.GetS3Path(path);
		newPath = _pathUtils.GetS3Path(newPath);
		var objects = await Client.ListObjectsAsync(new ListObjectsRequest() {
			BucketName = _options.BucketName,
			Prefix = path
		});
		var copyRequests = objects.S3Objects.Select(x => new CopyObjectRequest {
			SourceBucket = _options.BucketName,
			SourceKey = x.Key,
			DestinationBucket = _options.BucketName,
			DestinationKey = x.Key.Replace(path, newPath)
		}).ToList();
		var copyTasks = copyRequests.Select(x => Client.CopyObjectAsync(x)).ToList();
		await Task.WhenAll(copyTasks);
		await DeleteFolderAsync(path);
	}

	public async Task DeleteFolderAsync (string path) {
		path = _pathUtils.GetS3Path(path);
		var objects = await Client.ListObjectsAsync(new ListObjectsRequest() {
			BucketName = _options.BucketName,
			Prefix = path
		});
		await Client.DeleteObjectsAsync(new DeleteObjectsRequest() {
			BucketName = _options.BucketName,
			Objects = objects.S3Objects.Select(x => new KeyVersion() {
				Key = x.Key
			}).ToList(),
		});
		
	}

	public async Task CreateFileAsync (string path, byte[] content) {
		path = _pathUtils.GetS3Path(path);
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
		path = _pathUtils.GetS3Path(path);
		await Client.DeleteObjectAsync(new DeleteObjectRequest {
			BucketName = _options.BucketName,
			Key = path
		});
	}

	public async Task<byte[]> GetFileContentAsync (string path) {
		path = _pathUtils.GetS3Path(path);
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
		path = _pathUtils.GetS3Path(path);
		var response = await Client.ListObjectsAsync(new ListObjectsRequest {
			BucketName = _options.BucketName,
			Prefix = path
		});
		return response.S3Objects.Any();
	}

	public async Task<bool> FileExistsAsync (string path) {
		path = _pathUtils.GetS3Path(path);
		return await FolderExistsAsync(path);
	}

	public async Task<FilesystemEntry> GetFilesystemEntryInfo (string path) {
		path = _pathUtils.GetS3Path(path);
		var response = await Client.ListObjectsAsync(new ListObjectsRequest {
			BucketName = _options.BucketName,
			Prefix = path
		});
		var entry = await GetFilesystemEntriesByS3Objects(response.S3Objects, path);
		return entry.FirstOrDefault()!;
	}

	private async Task<IEnumerable<FilesystemEntry>> GetFilesystemEntriesByS3Objects (IEnumerable<S3Object> objects, string path) {
		HashSet<FilesystemEntry> folders = new ();
		HashSet<FilesystemEntry> files = new ();
		foreach (var s3Object in objects) {
			if (s3Object.Key.EndsWith('/')) {
				//is folder
				string folderName = _pathUtils.GetS3DirectoryFromPath(s3Object.Key, path);
				folders.Add(new FilesystemEntry(folderName.Trim('/'), '/' + path, true, -1, s3Object.LastModified.ToUniversalTime()));
			} else {
				//is file
				var meta = await Client.GetObjectMetadataAsync(new GetObjectMetadataRequest {
					BucketName = _options.BucketName,
					Key = s3Object.Key
				});
				files.Add(new FilesystemEntry(s3Object.Key.Trim('/'), '/' + path, false, meta.Headers.ContentLength, s3Object.LastModified.ToUniversalTime()));
			}
		}
		return folders.Concat(files);
	}

	public void Disconnect () {
		_client?.Dispose();
	}
}