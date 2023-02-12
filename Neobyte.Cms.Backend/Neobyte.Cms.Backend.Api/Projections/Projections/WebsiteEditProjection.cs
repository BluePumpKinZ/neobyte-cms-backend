using AutoMapper;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Projections.Projections; 

public class WebsiteEditProjection : IProjection {

	public WebsiteId Id { get; set; }
	public string Name { get; set; } = string.Empty;
	

	public void RegisterMap (IMapperConfigurationExpression configuration) {
		configuration.CreateMap<Website, WebsiteEditProjection>();
	}

}