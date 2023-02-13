using AutoMapper;
using Neobyte.Cms.Backend.Domain.Websites;
using Neobyte.Cms.Backend.Domain.Websites.HostingConnections;

namespace Neobyte.Cms.Backend.Api.Projections.Projections; 

public class WebsiteEditProjection : IProjection {

	public WebsiteId Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Domain { get; set; } = string.Empty;
	public string Protocol { get; set; } = string.Empty;
	public string Host { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public int Port { get; set; }

	public void RegisterMap (IMapperConfigurationExpression configuration) {
		configuration.CreateMap<Website, WebsiteEditProjection>().ConvertUsing<WebsiteEditProjectionConverter>();
	}

	class WebsiteEditProjectionConverter : ITypeConverter<Website, WebsiteEditProjection> {

		public WebsiteEditProjection Convert (Website source, WebsiteEditProjection? destination, ResolutionContext context) {
			destination ??= new WebsiteEditProjection {
				Id = source.Id,
				Name = source.Name,
				Domain = source.Domain,
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