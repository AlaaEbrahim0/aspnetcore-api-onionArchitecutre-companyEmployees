using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs;
public abstract record EmployeeForCreationOrUpdation
{
	[Required(ErrorMessage = "Employee name is a required field.")]
	[MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
	public string? Name { get; init; }

	[Required(ErrorMessage = "Age is a required field.")]
	[Range(18, 60, ErrorMessage = "Age must be between [18 - 60]")]
	public int Age { get; init; }

	[Required(ErrorMessage = "Position is a required field.")]
	[MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
	public string? Position { get; init; }
}

