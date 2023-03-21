using Bogus;
using Neobyte.Cms.Backend.Core.Accounts.Models;
using Neobyte.Cms.Backend.Core.Identity;
using Neobyte.Cms.Backend.Core.Websites.Models;

namespace Neobyte.Cms.Backend.Tests; 

public class Fakers {

	public Faker<AccountsWithPasswordCreateRequestModel> Accounts { get; }
	public Faker<WebsiteCreateRequestModel> Websites { get; }

	public Fakers() {
		Accounts = new Faker<AccountsWithPasswordCreateRequestModel>()
			.RuleFor(x => x.Email, f => f.Person.Email)
			.RuleFor(x => x.Username, f => f.Person.UserName)
			.RuleFor(x => x.Bio, f => f.Lorem.Sentence())
			.RuleFor(x => x.Password, f => f.Internet.PasswordCustom())
			.RuleFor(x => x.Role, f => f.PickRandom(Role.All).RoleName);

		Websites = new Faker<WebsiteCreateRequestModel>()
			.RuleFor(x => x.Name, f => f.Company.CompanyName())
			.RuleFor(x => x.Domain, f => "https://neobyte.net/thelab")
			.RuleFor(x => x.HomeFolder, f => f.System.DirectoryPath())
			.RuleFor(x => x.UploadFolder, f => f.System.DirectoryPath())
			.RuleFor(x => x.Protocol, "FTP")
			.RuleFor(x => x.Host, f => f.Internet.Ip())
			.RuleFor(x => x.Port, f => f.Random.Int(1, 65535))
			.RuleFor(x => x.Username, f => f.Person.UserName)
			.RuleFor(x => x.Password, f => f.Internet.PasswordCustom());
	}

}