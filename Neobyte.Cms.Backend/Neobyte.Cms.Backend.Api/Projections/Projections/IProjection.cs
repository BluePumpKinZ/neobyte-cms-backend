using AutoMapper;

namespace Neobyte.Cms.Backend.Api.Projections.Projections; 

internal interface IProjection {

	void RegisterMap (IMapperConfigurationExpression configuration);

}