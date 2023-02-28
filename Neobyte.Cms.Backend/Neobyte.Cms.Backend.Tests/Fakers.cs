using Bogus;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Identity;

namespace Neobyte.Cms.Backend.Tests; 

public class Fakers {

	public Faker<AccountsWithPasswordCreateRequestModel> Accounts { get; }

	public Fakers() {
		Accounts = new Faker<AccountsWithPasswordCreateRequestModel>()
			.RuleFor(x => x.Email, f => f.Person.Email)
			.RuleFor(x => x.Username, f => f.Person.UserName)
			.RuleFor(x => x.Bio, f => f.Lorem.Sentence())
			.RuleFor(x => x.Password, f => f.Internet.PasswordCustom())
			.RuleFor(x => x.Role, f => f.PickRandom(Role.All).RoleName);
	}

}