using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs;

public record CompanyDto
{
    public int Id { get; init; }	
    public string? Name { get; init; }	
    public string? FullAddress { get; init; }	
}