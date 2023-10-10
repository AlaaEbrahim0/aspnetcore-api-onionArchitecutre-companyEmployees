using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController: ControllerBase
{
	private readonly IServiceManager serviceManager;

	public CompanyController(IServiceManager serviceManager)
    {
		this.serviceManager = serviceManager;
	}

    [HttpGet]
	public IActionResult GetCompanies()
	{
		try
		{
			var companies = serviceManager.CompanyService.GetAllCompanies(false);
			return Ok(companies);

		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
		}
	}

}
