using AutoMapper;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Projections.Projections; 

internal class WebsiteProjection : IProjection {

	public WebsiteId Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Domain { get; set; } = string.Empty;
	public string Screenshot => $"http://localhost:5110/api/v1/websites/{Id}/thumbnail";
	public DateTime CreatedDate { get; set; }

	public void RegisterMap (IMapperConfigurationExpression configuration) {
		configuration.CreateMap<Website, WebsiteProjection>();
	}

}