using AutoMapper;

namespace Neobyte.Cms.Backend.Api.Projections.Projections; 

public interface IProjection {

	void RegisterMap (IMapperConfigurationExpression configuration);

}