using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace Services.DataShaping;
public class DataShaper<T> : IDataShaper<T> where T : class
{
	public PropertyInfo[] Properties { get; set; }

    public DataShaper()
    {
        Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entites, string fieldsString)
	{
		var requiredProperties = GetRequiredProperties(fieldsString);
		return FetchData(entites, requiredProperties);
	}

	public ExpandoObject ShapeData(T entity, string fieldsString)
	{
		var requiredProperties = GetRequiredProperties(fieldsString);
		return FetchDataForEntity(entity, requiredProperties);
	}

	private IEnumerable<ExpandoObject> FetchData
		(IEnumerable<T> entites, IEnumerable<PropertyInfo> requiredProperties)
	{
		var shapedData = new List<ExpandoObject>();
		foreach (var entity in entites)
		{
			var shapedObject = FetchDataForEntity(entity, requiredProperties);
			shapedData.Add(shapedObject);
		}
		return shapedData;
	}

	private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
	{
		var shapedObject = new ExpandoObject();
		foreach (var property in requiredProperties)
		{
			var objectPropertyValue = property.GetValue(entity);
			shapedObject.TryAdd(property.Name, objectPropertyValue);
		}
		return shapedObject;
	}

	private IEnumerable<PropertyInfo> GetRequiredProperties (string fieldsString)
	{
		var propertiesList = new List<PropertyInfo> ();
		if (string.IsNullOrWhiteSpace(fieldsString))
		{
			propertiesList = Properties.ToList();
			return propertiesList;
		}

		var fields = fieldsString.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);
		foreach (var field in fields)
		{
			var property = Properties.
				FirstOrDefault(p => p.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));
			
			if (property is not null)
			{
				propertiesList.Add(property);
			}
		}
		return propertiesList;
	}
}
