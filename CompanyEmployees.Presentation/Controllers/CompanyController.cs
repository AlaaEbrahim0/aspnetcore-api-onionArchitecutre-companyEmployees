using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace CompanyEmployees.Presentation.Controllers;

[ApiController]
[Route("api/companies")]
[ResponseCache(CacheProfileName = "120SecondsDuration")]
public class CompanyController: ControllerBase
{
	private readonly IServiceManager _serviceManager;

	public CompanyController(IServiceManager serviceManager)
    {
		this._serviceManager = serviceManager;
	}

	[HttpOptions]
	public IActionResult GetCompaniesOptions()
	{
		HttpContext.Response.Headers.Add("Allow", "GET, POST, PUT, PATCH, DELETE, OPTIONS, HEAD");
		return Ok();
	}

    [HttpGet]
	[HttpHead]
	[Authorize]
	public async Task<IActionResult> GetCompaniesAsync([FromQuery] CompanyParameters companyParameters)
	{
		var companies = await _serviceManager.CompanyService.GetAllCompaniesAsync(companyParameters ,false);
		return Ok(companies);		
	}

	[HttpGet("collection/{ids}")]
	[Route("CompanyCollection")]
	public async Task<IActionResult> GetCompanyCollectionAsync(IEnumerable<int> ids)
	{
		var companies = await _serviceManager.CompanyService.GetByIdsAsync(ids, false);
		return Ok(companies);
	}

	[HttpGet("{id:int}", Name = "CompanyById")]
	public async Task<IActionResult> GetCompanyAsync(int id)
	{
		var company = await _serviceManager.CompanyService.GetCompanyAsync(id, false);
		return Ok(company);
	}

	[HttpPost]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	public async Task<IActionResult> CreateCompanyAsync([FromBody] CompanyForCreationDto companyDto)
	{
		var company = await _serviceManager.CompanyService.CreateCompanyAsync(companyDto);
		return CreatedAtRoute("CompanyById", new { id = company?.Id }, company);
	}

	[HttpPost("collection")]
	public async Task<IActionResult> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companies)
	{
		if (!ModelState.IsValid)
			return UnprocessableEntity(ModelState);

		var result = await _serviceManager.CompanyService.CreateCompanyCollectionAsync(companies);
		return CreatedAtRoute("CompanyCollection", new { ids = result.ids }, result.companies);
	}

	[HttpDelete("{companyId:int}")]
	public async Task<IActionResult> DeleteCompanyAsync(int companyId)
	{
		await _serviceManager.CompanyService.DeleteCompanyAsync(companyId, false);
		return Ok($"Company with Id: {companyId} has been deleted successfully");
	}

	[HttpPut("{companyId:int}")]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	public async Task<IActionResult> UpdateCompanyAsync(int companyId, CompanyForUpdationDto companyToUpdate)
	{
		await _serviceManager.CompanyService.UpdateCompanyAsync(companyId, companyToUpdate, true);
		return Ok($"Company with Id: {companyId} has been updated successfully");
	}

	[HttpPatch("{companyId:int}")]
	public async Task<IActionResult> PartiallyUpdateCompanyAsync(int companyId, JsonPatchDocument<CompanyForUpdationDto> patchDocument)
	{
		if (patchDocument is null)
			return BadRequest("CompanyForUpdation Dto is null");

		var result = await _serviceManager.CompanyService.GetCompanyForPatchAsync(companyId, true);

		patchDocument.ApplyTo(result.companyForPatch, ModelState);

		TryValidateModel(ModelState);

		if (!ModelState.IsValid)
			return UnprocessableEntity(ModelState);

		await _serviceManager.CompanyService.SaveChangesForPatchAsync(result.companyForPatch, result.companyEntity);
		return NoContent();

	}

}
