using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;

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

	[HttpPost]
	public IActionResult CreateEmployee(int companyId, CreateEmployeeDto employeeForCreation)
	{
		if (employeeForCreation is null)
			return BadRequest("CreateEmployeeDto is null");

		var employee = serviceManager.EmployeeService.CreateEmployeeForCompany(companyId, employeeForCreation, true);
		return CreatedAtAction(nameof(GetEmployee), new { companyId, employeeId = employee.Id }, employee);
	}

	[HttpDelete("{employeeId:int}")]
	public IActionResult DeleteEmployeeForCompany(int companyId, int employeeId)
	{
		serviceManager.EmployeeService.DeleteEmployeeForCompany(companyId, employeeId, false);
		return Ok($"Employee with id: {employeeId} has been deleted successfully");
	}
}
