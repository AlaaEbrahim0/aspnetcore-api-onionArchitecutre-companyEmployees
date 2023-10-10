using Bogus;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
	public void Configure(EntityTypeBuilder<Company> builder)
	{
		int id = 1;
		var countries = new List<string>
		{
			"USA", "England", "France", "Germany", "Spain"
		};

		var companyFaker = new Faker<Company>()
			.RuleFor(c => c.Id, f => id++)
			.RuleFor(c => c.Name, f => f.Company.CompanyName())
			.RuleFor(c => c.Address, f => f.Address.StreetAddress())
			.RuleFor(c => c.Country, f => f.PickRandom(countries));

		var data = companyFaker.Generate(20);
		builder.HasData(data);
	}
}
