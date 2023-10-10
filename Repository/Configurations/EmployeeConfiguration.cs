using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
	public void Configure(EntityTypeBuilder<Employee> builder)
	{
		int id = 1;

		var employeeFaker = new Faker<Employee>()
			.RuleFor(e => e.Id, f => id++)
			.RuleFor(e => e.Name, f => f.Name.FullName())
			.RuleFor(e => e.Age, f => f.Random.Int(18, 60))
			.RuleFor(e => e.CompanyId, f => f.Random.Int(1, 20))
			.RuleFor(e => e.Position, f => f.Name.JobTitle());

		var data = employeeFaker.Generate(1000);
		builder.HasData(data);

	}
}
