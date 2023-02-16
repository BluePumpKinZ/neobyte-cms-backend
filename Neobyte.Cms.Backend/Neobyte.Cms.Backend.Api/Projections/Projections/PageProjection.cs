using AutoMapper;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Projections.Projections; 

internal class PageProjection : IProjection {

	public PageId Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Path { get; set; } = string.Empty;
	public DateTime Created { get; set; }
	public DateTime Modified { get; set; }

	public void RegisterMap (IMapperConfigurationExpression configuration) {
		configuration.CreateMap<Page, PageProjection>();
	}

}