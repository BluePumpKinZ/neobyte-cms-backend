using StronglyTypedIds;
using System;
using System.ComponentModel.DataAnnotations;

namespace Neobyte.Cms.Backend.Domain.Accounts;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct AccountId {}

public class Account {

	[Key]
	public AccountId Id { get; set; }
	[Required]
	public string Firstname { get; set; }
	[Required]
	public string Lastname { get; set; }
	[Required]
	public DateTime CreationDate { get; set; }

	public Account (string firstname, string lastname) {
		Id = AccountId.New();
		CreationDate = DateTime.UtcNow;
		Firstname = firstname;
		Lastname = lastname;
	}

	public Account (AccountId id, string firstname, string lastname, DateTime creationDate) {
		Id = id;
		Firstname = firstname;
		Lastname = lastname;
		CreationDate = creationDate;
	}

}