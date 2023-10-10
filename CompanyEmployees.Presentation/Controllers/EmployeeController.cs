using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyId:int}/employees")]
[ApiController]
public class EmployeeController : ControllerBase
{
	private readonly IServiceManager serviceManager;

	public EmployeeController(IServiceManager serviceManager)
	{
		this.serviceManager = serviceManager;
	}

	[HttpGet]
	public IActionResult GetEmployees(int companyId)
	{
		var employees = serviceManager.EmployeeService.GetEmployees(companyId, false);
		return Ok(employees);
	}

	[HttpGet("{employeeId:int}")]
	public IActionResult GetEmployee(int companyId, int employeeId)
	{
		var employee = serviceManager.EmployeeService.GetEmployee(companyId, employeeId, false);
		return Ok(employee);
	}
}
