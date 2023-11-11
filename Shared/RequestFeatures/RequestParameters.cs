using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures;
public abstract class RequestParameters
{
	private const int maxPageSize = 50;
	public int PageNumber { get; set; } = 1;

	private int pageSize = 10;
	public int PageSize
	{
		get
		{
			return pageSize;
		}
		set
		{
			pageSize = (value > maxPageSize) ? maxPageSize : value;
		}
	}

	public string OrderBy { get; set; } = string.Empty;
	public string Fields { get; set; } = string.Empty;
}
