using AutoMapper;
using System.Collections.Generic;

namespace Neobyte.Cms.Backend.Api.Projections; 

internal class ProjectionMapperFactory {

	private readonly IEnumerable<IProjection> _projections;

	public ProjectionMapperFactory (IEnumerable<IProjection> projections) {
		_projections = projections;
	}

	public IMapper CreateMapper () {

		var configuration = new MapperConfiguration(cfg => {
			foreach (var projection in _projections) {
				projection.RegisterMap(cfg);
			}
		});

		return configuration.CreateMapper();
	}

}