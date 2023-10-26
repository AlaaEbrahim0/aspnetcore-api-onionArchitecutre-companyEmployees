using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
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

	[HttpGet("collection/{ids}")]
	public IActionResult GetCompanyCollection(IEnumerable<int> ids)
	{
		var companies = serviceManager.CompanyService.GetByIds(ids, false);
		return Ok(companies);
	}

	[HttpGet("{id:int}", Name = "CompanyById")]
	public IActionResult GetCompany(int id)
	{
		var company = serviceManager.CompanyService.GetCompany(id, false);
		return Ok(company);
	}

	[HttpPost]
	public IActionResult CreateCompany([FromBody] CompanyForCreationDto companyDto)
	{
		if (companyDto is null)
			return BadRequest("CompanyForCreationDto is null");

		if (!ModelState.IsValid)
			return UnprocessableEntity(ModelState);

		var company = serviceManager.CompanyService.CreateCompany(companyDto);

		return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company);
	}

	[HttpPost("collection")]
	public IActionResult CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companies)
	{
		if (!ModelState.IsValid)
			return UnprocessableEntity(ModelState);

		var result = serviceManager.CompanyService.CreateCompanyCollection(companies);
		return CreatedAtAction(nameof(GetCompanyCollection), new { ids = result.ids }, result.companies);
	}

	[HttpDelete("{companyId:int}")]
	public IActionResult DeleteCompany(int companyId)
	{
		serviceManager.CompanyService.DeleteCompany(companyId, false);
		return Ok($"Company with Id: {companyId} has been deleted successfully");
	}

	[HttpPut("{companyId:int}")]
	public IActionResult UpdateCompany(int companyId, CompanyForUpdationDto companyToUpdate)
	{
		if (companyToUpdate is null)
			return BadRequest("CompanyForUpdationDto is null");

		if (!ModelState.IsValid)
			return UnprocessableEntity(ModelState);

		serviceManager.CompanyService.UpdateCompany(companyId, companyToUpdate, true);
		return Ok($"Company with Id: {companyId} has been updated successfully");
	}

	[HttpPatch("{companyId:int}")]
	public IActionResult PartiallyUpdateCompany(int companyId, JsonPatchDocument<CompanyForUpdationDto> patchDocument)
	{
		if (patchDocument is null)
			return BadRequest("CompanyForUpdation Dto is null");

		var result = serviceManager.CompanyService.GetCompanyForPatch(companyId, true);

		patchDocument.ApplyTo(result.companyForPatch, ModelState);

		TryValidateModel(ModelState);

		if (!ModelState.IsValid)
			return UnprocessableEntity(ModelState);

		serviceManager.CompanyService.SaveChangesForPatch(result.companyForPatch, result.companyEntity);
		return NoContent();

	}

}
