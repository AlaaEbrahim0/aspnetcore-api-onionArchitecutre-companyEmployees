using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs;
public record CreateCompanyDto
{
    public string? Name { get; init; }
    public string? Address{ get; init; }
    public string? Country { get; init; }
    public IEnumerable<CreateEmployeeDto> Employees{ get; init; }
}
