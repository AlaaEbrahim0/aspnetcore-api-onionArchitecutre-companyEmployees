using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs;
public record CompanyDto
(
	int Id,
	string Name,
	string FullAddress
);
