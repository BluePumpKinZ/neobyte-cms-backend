using AutoMapper;
using Neobyte.Cms.Backend.Domain.Accounts;

namespace Neobyte.Cms.Backend.Api.Projections.Projections; 

public class AccountProjection : IProjection {

	public AccountId Id { get; set; }
	public string Email { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Bio { get; set; } = string.Empty;
	public DateTime CreationDate { get; set; }
	public string[]? Roles { get; set; }

	public void RegisterMap (IMapperConfigurationExpression configuration) {
		configuration.CreateMap<Account, AccountProjection>();
	}

}