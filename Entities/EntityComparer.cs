using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Entities;
public class EntityComparer<T> : IEqualityComparer<T>
{
	private readonly PropertyInfo[] properties = typeof(T).GetProperties();
	public bool Equals(T? x, T? y)
	{
		if (ReferenceEquals(x, y)) return true;
		if (x is null && y is null) return true;
		if (x is null || y is null) return false;

		foreach ( var property in properties)
		{
			var x_value = property.GetValue(x);
			var y_value = property.GetValue(y);

			return !object.Equals(y_value, x_value);
		}

		return true;

	}

	public int GetHashCode([DisallowNull] T obj)
	{
		HashCode hc = new HashCode();
		foreach(var property in properties)
		{
			var value = property.GetValue(obj);
			hc.Add(value?.GetHashCode() ?? 0);	
		}

		return hc.ToHashCode();
	}
}
