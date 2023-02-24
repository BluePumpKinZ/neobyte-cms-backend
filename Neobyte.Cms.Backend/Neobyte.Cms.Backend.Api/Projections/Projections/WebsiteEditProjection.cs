using AutoMapper;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Api.Projections.Projections; 

internal class WebsiteEditProjection : IProjection {

	public WebsiteId Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Domain { get; set; } = string.Empty;
	public string HomeFolder { get; set; } = string.Empty;
	public string UploadFolder { get; set; } = string.Empty;
	public string Protocol { get; set; } = string.Empty;
	public string Host { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public int Port { get; set; }

	public void RegisterMap (IMapperConfigurationExpression configuration) {
		configuration.CreateMap<Website, WebsiteEditProjection>().ConvertUsing<WebsiteEditProjectionConverter>();
	}

	private class WebsiteEditProjectionConverter : ITypeConverter<Website, WebsiteEditProjection> {

		public WebsiteEditProjection Convert (Website source, WebsiteEditProjection? destination, ResolutionContext context) {
			destination ??= new WebsiteEditProjection {
				Id = source.Id,
				Name = source.Name,
				Domain = source.Domain,
				HomeFolder = source.HomeFolder,
				UploadFolder = source.UploadFolder
			};
			switch(source.Connection?.GetType()) {
			case var value when value == typeof(FtpHostingConnection):
				var ftpConnection = (FtpHostingConnection)source.Connection!;
				destination.Host = ftpConnection.Host;
				destination.Username = ftpConnection.Username;
				destination.Password = ftpConnection.Password;
				destination.Port = ftpConnection.Port;
				destination.Protocol = "FTP";
				break;
			case var value when value == typeof(SftpHostingConnection):
				var sftpConnection = (SftpHostingConnection)source.Connection!;
				destination.Host = sftpConnection.Host;
				destination.Username = sftpConnection.Username;
				destination.Password = sftpConnection.Password;
				destination.Port = sftpConnection.Port;
				destination.Protocol = "SFTP";
				break;
			}
			return destination;
		}

	}

}