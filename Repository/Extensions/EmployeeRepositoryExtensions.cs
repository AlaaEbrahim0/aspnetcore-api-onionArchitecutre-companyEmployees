using System.Reflection;
using System.Linq.Dynamic.Core;
using System.Text;
using Entities.Models;

namespace Repository.Extensions;

public static partial class EmployeeRepositoryExtensions
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
    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
        {
            return employees.OrderBy(e => e.Name);
        }

        var orderParams = orderBy.Trim().Split(',');
        var propertyInfos = typeof(Employee)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var orderByQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;

            var propertyNameFromQueryString = param.Trim().Split(' ')[0];
            var objProperty = propertyInfos
                .FirstOrDefault(pi => pi.Name.Equals(propertyNameFromQueryString, StringComparison.InvariantCultureIgnoreCase));

            if (objProperty is not null)
            {
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderByQueryBuilder.Append($"{objProperty.Name} {direction},");
            }
        }
        var orderByQuery = orderByQueryBuilder.ToString().TrimEnd(',', ' ');

        if (string.IsNullOrWhiteSpace(orderByQuery))
            return employees.OrderBy(e => e.Name);

        return employees.OrderBy(orderByQuery);
    }

}
