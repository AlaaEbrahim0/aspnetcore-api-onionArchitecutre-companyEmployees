using Entities;

namespace Repository.Extensions;


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
