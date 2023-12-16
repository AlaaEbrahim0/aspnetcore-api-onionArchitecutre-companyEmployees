using System.Text.Json;
using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

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
	[HttpHead]
	public async Task<IActionResult> GetEmployeesAsync(int companyId, [FromQuery] EmployeeParameters employeeParameters)
	{ 
		var employeesPagedResult = await serviceManager.EmployeeService.GetEmployeesAsync(companyId, employeeParameters, false);
		Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(employeesPagedResult.metaData));

        return Ok(employeesPagedResult.employees);
	}

	[HttpGet("{employeeId:int}")]
	public async Task<IActionResult> GetEmployee(int companyId, int employeeId)
	{
		EmployeeDto employee = await serviceManager.EmployeeService.GetEmployeeAsync(companyId, employeeId, false);
		return Ok(employee);
	}

	[HttpPost]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	public async Task<IActionResult> CreateEmployeeAsync(int companyId, EmployeeForCreationDto employeeForCreation)
	{
		var employee = await serviceManager.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employeeForCreation, true);
		return CreatedAtAction(nameof(GetEmployee), new { companyId, employeeId = employee.Id }, employee);
	}

	[HttpDelete("{employeeId:int}")]
	public async Task<IActionResult> DeleteEmployeeForCompanyAsync(int companyId, int employeeId)
	{
		await serviceManager.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, employeeId, true);
		return Ok($"Employee with id: {employeeId} has been deleted successfully");
	}

	[HttpPut("{employeeId:int}")]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	public async Task<IActionResult> UpdateEmployeeForCompanyAsync(int companyId, int employeeId, EmployeeForUpdationDto employeeForUpdate)
	{	
		await serviceManager.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, employeeId, employeeForUpdate, false, true);
		return Ok($"Employee with ID: {employeeId} has been updated successfully");
	}

	[HttpPatch("{employeeId:int}")]
	public async Task<IActionResult> PartiallyUpdateEmployeeForCompanyAsync
		(int companyId, int employeeId, [FromBody] JsonPatchDocument<EmployeeForUpdationDto> patchDocument)
	{
		if (patchDocument is null)
			return BadRequest("Employee Patch Document is null");

		var result = await serviceManager.EmployeeService
			.GetEmployeeForPatchAsync(companyId, employeeId, false, true);

		patchDocument.ApplyTo(result.employeeToPatch, ModelState);

		TryValidateModel(result.employeeToPatch);

		if (!ModelState.IsValid)
			return UnprocessableEntity(ModelState);
		
		await serviceManager.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);

		return NoContent();
	}
}
