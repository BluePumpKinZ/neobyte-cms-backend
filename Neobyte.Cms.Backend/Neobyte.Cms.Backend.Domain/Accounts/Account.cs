using StronglyTypedIds;
using System.ComponentModel.DataAnnotations;

namespace Neobyte.Cms.Backend.Domain.Accounts;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct AccountId {}

public class Account {

	[Key]
	public AccountId Id { get; set; }

}