using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Entities;
using Shared.RequestFeatures;

namespace Repository;

public static class EmployeeRepositoryExtensions
{
	public static IQueryable<Employee> Filter(this IQueryable<Employee> employees, int minAge, int maxAge)
	{
		return employees.Where(e => e.Age >= minAge && e.Age <= maxAge);
	}
	public static IQueryable<Employee> SearchByName(this IQueryable<Employee> employees, string searchTerm)
	{
		if (string.IsNullOrWhiteSpace(searchTerm))
			return employees;

		var lowercasedTrimmedSearchTerm = searchTerm.Trim().ToLower();
		return employees.Where(e => e.Name!.Equals(lowercasedTrimmedSearchTerm));
	}
}


public static class CompanyRepositoryExtensions
{
	public static IQueryable<Company> Filter(this IQueryable<Company> companies, string address)
	{
		return companies.Where(c => c.Address!.Contains(address));
	}
	public static IQueryable<Company> SearchByName(this IQueryable<Company> companies, string searchTerm)
	{
		if (string.IsNullOrWhiteSpace(searchTerm))
			return companies;

		var lowercasedTrimmedSearchTerm = searchTerm.Trim().ToLower();
		return companies.Where(e => e.Name!.Equals(lowercasedTrimmedSearchTerm));
	}
}
