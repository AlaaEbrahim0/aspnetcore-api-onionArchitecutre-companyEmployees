using System.Dynamic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace CompanyEmployees.CustomFormatters;

public class CsvOutputFormatter<T> : TextOutputFormatter 
{
    public CsvOutputFormatter()
    {
        this.SupportedMediaTypes.Add("text/csv");
        this.SupportedEncodings.Add(Encoding.UTF8);
        this.SupportedEncodings.Add(Encoding.Unicode);
	}

	protected override bool CanWriteType(Type? type)
	{
		if (typeof(Type) == typeof(ExpandoObject))
		{
            return base.CanWriteType(type);
		}

		if (typeof(T).IsAssignableFrom(type) || 
            typeof(IEnumerable<T>).IsAssignableFrom(type))
        {
            return base.CanWriteType(type);
        }
        return false;
	}

	public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
	{
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        if (context.Object is IEnumerable<T>) 
        {
            foreach(var entity in (IEnumerable<T>) context.Object) 
            {
                FormatCsv(buffer, entity);
            }
        }
        else
        {
			FormatCsv(buffer, (T)context.Object);
		}

        await response.WriteAsync(buffer.ToString());
	}
	private static void FormatCsv(StringBuilder buffer, T entity)
	{
		if (entity is ExpandoObject expandoObject)
		{
			var dictionary = expandoObject as IDictionary<string, object>;

			if (dictionary != null)
			{
				foreach (var keyValuePair in dictionary)
				{
					var value = keyValuePair.Value;
					buffer.Append($"{value},");
				}
			}
		}
		else
		{
			var properties = entity.GetType().GetProperties();
			foreach (var property in properties)
			{
				var value = property.GetValue(entity);
				buffer.Append($"{value},");
			}
		}

		buffer.Length--;
		buffer.AppendLine();
	}

}
