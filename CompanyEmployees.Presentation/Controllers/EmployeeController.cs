using Microsoft.AspNetCore.JsonPatch;
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
	public IActionResult CreateEmployee(int companyId, EmployeeForCreationDto employeeForCreation)
	{
		if (employeeForCreation is null)
			return BadRequest("EmployeeForCreationDto is null");
		
		if (!ModelState.IsValid)
		{
			return UnprocessableEntity(ModelState);
		}

		var employee = serviceManager.EmployeeService.CreateEmployeeForCompany(companyId, employeeForCreation, true);
		return CreatedAtAction(nameof(GetEmployee), new { companyId, employeeId = employee.Id }, employee);
	}

	[HttpDelete("{employeeId:int}")]
	public IActionResult DeleteEmployeeForCompany(int companyId, int employeeId)
	{
		serviceManager.EmployeeService.DeleteEmployeeForCompany(companyId, employeeId, false);
		return Ok($"Employee with id: {employeeId} has been deleted successfully");
	}

	[HttpPut("{employeeId:int}")]
	public IActionResult UpdateEmployeeForCompany(int companyId, int employeeId, EmployeeForUpdationDto employeeForUpdate)
	{	
		if (employeeForUpdate is null)
			return BadRequest("EmployeeForUpdationDto is null");

		if (!ModelState.IsValid)
			return UnprocessableEntity(ModelState);

		serviceManager.EmployeeService.UpdateEmployeeForCompany(companyId, employeeId, employeeForUpdate, false, true);
		return Ok($"Employee with ID: {employeeId} has been updated successfully");
	}

	[HttpPatch("{employeeId:int}")]
	public IActionResult PartiallyUpdateEmployeeForCompany
		(int companyId, int employeeId, [FromBody] JsonPatchDocument<EmployeeForUpdationDto> patchDocument)
	{
		if (patchDocument is null)
			return BadRequest("Employee Patch Document is null");

		var result = serviceManager.EmployeeService
			.GetEmployeeForPatch(companyId, employeeId, false, true);

		patchDocument.ApplyTo(result.employeeToPatch, ModelState);

		TryValidateModel(result.employeeToPatch);

		if (!ModelState.IsValid)
			return UnprocessableEntity(ModelState);
		
		serviceManager.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);

		return NoContent();
	}
}
