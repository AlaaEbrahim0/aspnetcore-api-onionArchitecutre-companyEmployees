using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;
public class EmployeeInvalidAgeRangeException: Exception
{
    public int MinAge { get; set; }
    public int MaxAge { get; set; }

    
	public EmployeeInvalidAgeRangeException(int minAge, int maxAge)
        :base($"Invalid Age Range. MinAge: {minAge}, MaxAge: {maxAge}")
	{
		MinAge = minAge;
		MaxAge = maxAge;
	}
}
