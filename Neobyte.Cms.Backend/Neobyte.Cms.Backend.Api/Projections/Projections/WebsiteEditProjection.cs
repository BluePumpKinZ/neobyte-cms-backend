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
				UploadFolder = source.UploadFolder,
				Protocol = ""
			};
			switch(source.Connection?.GetType()) {
			case var value when value == typeof(FtpHostingConnection):
				var connection = (FtpHostingConnection)source.Connection!;
				destination.Host = connection.Host;
				destination.Username = connection.Username;
				destination.Password = connection.Password;
				destination.Port = connection.Port;
				destination.Protocol = "FTP";
				break;
			}
			return destination;
		}

	}

}