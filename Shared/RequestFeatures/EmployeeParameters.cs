using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures;
public class EmployeeParameters: RequestParameters
{
    private const int MinAgeAllowed = 18;
    private const int MaxAgeAllowed = 60;

	public uint MinAge { get; set; } = MinAgeAllowed;
    public uint MaxAge { get; set; } = MaxAgeAllowed;

    public bool ValidAgeRange => 
        MaxAge > MinAge && 
        MinAge >= MinAgeAllowed &&
        MaxAge <= MaxAgeAllowed;

    public string? SearchTerm = string.Empty!;
}
