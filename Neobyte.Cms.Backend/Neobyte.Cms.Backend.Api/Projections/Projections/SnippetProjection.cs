using AutoMapper;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Projections.Projections; 

internal class SnippetProjection : IProjection {

	public SnippetId Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	public void RegisterMap (IMapperConfigurationExpression configuration) {
		configuration.CreateMap<Snippet, SnippetProjection>();
	}

}