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
		var companies = serviceManager.CompanyService.GetAllCompanies(false);
		return Ok(companies);		
	}

	[HttpGet("{id:int}")]
	public IActionResult GetCompany(int id)
	{
		var company = serviceManager.CompanyService.GetCompany(id, false);
		return Ok(company);
	}

}
