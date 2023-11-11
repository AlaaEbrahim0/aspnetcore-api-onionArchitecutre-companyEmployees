using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Repository.Extensions.Utility;
public static class OrderByQueryBuilder
{
	public static string CreateOrderQuery<T>(string orderBy)
	{
		var orderParams = orderBy.Trim().Split(',');
		var propertyInfos = typeof(T)
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
		return orderByQuery;
	}
}
