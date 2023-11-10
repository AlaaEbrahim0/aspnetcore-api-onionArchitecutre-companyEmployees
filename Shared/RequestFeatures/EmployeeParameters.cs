using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures;
public class EmployeeParameters: RequestParameters
{
    public EmployeeParameters()
    {
        this.OrderBy = "name";
    }

    private const int MinAgeAllowed = 18;
    private const int MaxAgeAllowed = 60;

	public int MinAge { get; set; } = MinAgeAllowed;
    public int MaxAge { get; set; } = MaxAgeAllowed;

    public bool ValidAgeRange => 
        MaxAge > MinAge && 
        MinAge >= MinAgeAllowed &&
        MaxAge <= MaxAgeAllowed;

    public string SearchTerm = string.Empty;
}
