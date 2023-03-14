using Amazon.S3;
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
		var client = new AmazonS3Client(_options.AccessKey, _options.SecretKey, Amazon.RegionEndpoint.GetBySystemName(_options.Region));
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

	public Task<IEnumerable<FilesystemEntry>> ListItemsAsync (string path) {
		throw new NotImplementedException();
	}

	public Task CreateFolderAsync (string path) {
		throw new NotImplementedException();
	}

	public Task RenameFolderAsync (string path, string newPath) {
		throw new NotImplementedException();
	}

	public Task DeleteFolderAsync (string path) {
		throw new NotImplementedException();
	}

	public Task CreateFileAsync (string path, byte[] content) {
		throw new NotImplementedException();
	}

	public Task RenameFileAsync (string path, string newPath) {
		throw new NotImplementedException();
	}

	public Task DeleteFileAsync (string path) {
		throw new NotImplementedException();
	}

	public Task<byte[]> GetFileContentAsync (string path) {
		throw new NotImplementedException();
	}

	public Task<bool> FolderExistsAsync (string path) {
		throw new NotImplementedException();
	}

	public Task<bool> FileExistsAsync (string path) {
		throw new NotImplementedException();
	}

	public Task<FilesystemEntry> GetFilesystemEntryInfo (string path) {
		throw new NotImplementedException();
	}

	public void Disconnect () {
		throw new NotImplementedException();
	}
}