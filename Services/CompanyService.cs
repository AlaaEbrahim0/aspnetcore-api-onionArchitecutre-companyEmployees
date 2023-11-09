using System.Reflection.Metadata;
using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Services;
public class CompanyService: ICompanyService
{
	private readonly IRepositoryManager repository;
	private readonly ILoggerManager logger;
	private readonly IMapper mapper;

	public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
	{
		this.repository = repository;
		this.logger = logger;
		this.mapper = mapper;
	}

	public async Task<CompanyDto?> CreateCompanyAsync(CompanyForCreationDto CompanyForCreationDto)
	{
		var company = mapper.Map<Company>(CompanyForCreationDto);

		repository.Company.CreateCompany(company);
		await repository.SaveAsync();

		var returnCompanyDto = mapper.Map<CompanyDto>(company);

		return returnCompanyDto;	
	}

	public async Task DeleteCompanyAsync(int companyId, bool trackChanges)
	{
		Company? company = await getCompanyAndCheckIfExists(companyId, trackChanges);

		repository.Company.DeleteCompany(company);
		await repository.SaveAsync();
	}

	public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges)
	{
		var companies = await repository.Company
			.GetAllCompaniesAsync(companyParameters ,trackChanges);

		var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
		return companiesDto;
	}

	public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges)
	{
		if (ids is null)
			throw new IdParametersBadRequestException();

		var companies = await repository.Company.GetByIdsAsync(ids, trackChanges);
		if (ids.Count() != companies.Count())
			throw new CollectionByIdsBadRequestException();

		var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
		return companiesDto;
	}

	public async Task<CompanyDto?> GetCompanyAsync(int companyId, bool trackChanges)
	{
		Company? company = await getCompanyAndCheckIfExists(companyId, trackChanges);

		var companyDto = mapper.Map<CompanyDto>(company);

		return companyDto;
	}

	private async Task<Company> getCompanyAndCheckIfExists(int companyId, bool trackChanges)
	{
		var company = await repository.Company.GetCompanyAsync(companyId, trackChanges);

		if (company is null)
			throw new CompanyNotFoundException(companyId);

		return company;
	}

	public async Task<(CompanyForUpdationDto, Company)> GetCompanyForPatchAsync(int companyId, bool trackChanges)
	{
		Company? company = await getCompanyAndCheckIfExists(companyId, trackChanges);

		var companyForPatch = mapper.Map<CompanyForUpdationDto>(company);
		return (companyForPatch, company);
	}

	public async Task SaveChangesForPatchAsync(CompanyForUpdationDto updationDto, Company companyEntity)
	{
		mapper.Map(updationDto, companyEntity);
		await repository.SaveAsync();
	}

	public async Task UpdateCompanyAsync(int companyId, CompanyForUpdationDto companyForUpdate, bool trackChanges)
	{
		Company? company = await getCompanyAndCheckIfExists(companyId, trackChanges);

		mapper.Map(company, companyForUpdate);
		await repository.SaveAsync();
	}

	async Task<(IEnumerable<CompanyDto> companies, string ids)> ICompanyService.CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
	{
		if (companyCollection is null)
			throw new CompanyCollectionBadRequest();

		var companies = mapper.Map<IEnumerable<Company>>(companyCollection);
		foreach (var company in companies)
		{
			repository.Company.CreateCompany(company);
		}
		await repository.SaveAsync();

		var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
		var ids = string.Join(',', companies.Select(x => x.Id));
		
		return (companiesDto, ids);

	}

	
}
