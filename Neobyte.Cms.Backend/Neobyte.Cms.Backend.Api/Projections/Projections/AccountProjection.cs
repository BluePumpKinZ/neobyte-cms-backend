using AutoMapper;
using Neobyte.Cms.Backend.Domain.Accounts;
using System;

namespace Neobyte.Cms.Backend.Api.Projections.Projections; 

public class AccountProjection : IProjection {

	public AccountId Id { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public DateTime CreationDate { get; set; }

	public void RegisterMap (IMapperConfigurationExpression configuration) {
		configuration.CreateMap<Account, AccountProjection>();
	}

}