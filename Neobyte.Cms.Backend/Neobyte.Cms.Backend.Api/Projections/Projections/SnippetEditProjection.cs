using AutoMapper;
using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Api.Projections.Projections; 

public class SnippetEditProjection : IProjection {

	public SnippetId Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string? Html { get; set; } = string.Empty;

	public void RegisterMap (IMapperConfigurationExpression configuration) {
		configuration.CreateMap<Snippet, SnippetEditProjection>().ConvertUsing<SnippetProjectionConverter>();
	}

	private class SnippetProjectionConverter : ITypeConverter<Snippet, SnippetEditProjection> {

		public SnippetEditProjection Convert (Snippet source, SnippetEditProjection? destination, ResolutionContext context) {
			destination ??= new SnippetEditProjection {
				Id = source.Id,
				Name = source.Name,
				Description = source.Description,
			};

			destination.Html = source.Content!.Html;
			return destination;
		}


	}

}