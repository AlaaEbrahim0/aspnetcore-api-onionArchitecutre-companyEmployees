using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs;
public abstract record CompanyForCreationOrUpdation
{
	[Required(ErrorMessage = "Company name is required")]
	[MaxLength(30, ErrorMessage = "Company name cannot be more than 30 characters")]
	public string? Name { get; init; }

	[Required(ErrorMessage = "Company Address is required")]
	[MaxLength(255, ErrorMessage = "Company Address cannot be more than 255 characters")]
	public string? Address { get; init; }

	[Required(ErrorMessage = "Country is required")]
	[MaxLength(56)]	
	public string? Country { get; init; }


}

