using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace Neobyte.Cms.Backend.Api.Projections; 

public class Projector {

	private readonly IMapper _mapper;

	public Projector (IMapper mapper) {
		_mapper = mapper;
	}

	public TDestination Project<TSource, TDestination> (TSource source) {
		return (TDestination)_mapper.Map(source, typeof(TSource), typeof(TDestination));
	}

	public IEnumerable<TDestination> Project<TSource, TDestination> (IEnumerable<TSource> source) {
		return source.Select(Project<TSource, TDestination>);
	}

}