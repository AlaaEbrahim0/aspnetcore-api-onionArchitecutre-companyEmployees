using System.Reflection.Metadata;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Services;
public class CompanyService: ICompanyService
{
	private readonly IRepositoryManager _repository;
	private readonly ILoggerManager _logger;
	private readonly IMapper _mapper;

	public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
	{
		this._repository = repository;
		this._logger = logger;
		this._mapper = mapper;
	}

	public async Task<CompanyDto?> CreateCompanyAsync(CompanyForCreationDto companyForCreationDto)
	{
		var company = _mapper.Map<Company>(companyForCreationDto);

		_repository.Company.CreateCompany(company);
		await _repository.SaveAsync();

		var returnCompanyDto = _mapper.Map<CompanyDto>(company);

		return returnCompanyDto;	
	}

	public async Task DeleteCompanyAsync(int companyId, bool trackChanges)
	{
		Company? company = await getCompanyAndCheckIfExists(companyId, trackChanges);

		_repository.Company.DeleteCompany(company);
		await _repository.SaveAsync();
	}

	public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges)
	{
		var companies = await _repository.Company
			.GetAllCompaniesAsync(companyParameters ,trackChanges);

		var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
		return companiesDto;
	}

	public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges)
	{
		if (ids is null)
			throw new IdParametersBadRequestException();	

		var companies = await _repository.Company.GetByIdsAsync(ids, trackChanges);
		if (ids.Count() != companies.Count())
			throw new CollectionByIdsBadRequestException();

		var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
		return companiesDto;
	}

	public async Task<CompanyDto?> GetCompanyAsync(int companyId, bool trackChanges)
	{
		Company? company = await getCompanyAndCheckIfExists(companyId, trackChanges);

		var companyDto = _mapper.Map<CompanyDto>(company);

		return companyDto;
	}

	private async Task<Company> getCompanyAndCheckIfExists(int companyId, bool trackChanges)
	{
		var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

		if (company is null)
			throw new CompanyNotFoundException(companyId);

		return company;
	}

	public async Task<(CompanyForUpdationDto, Company)> GetCompanyForPatchAsync(int companyId, bool trackChanges)
	{
		Company? company = await getCompanyAndCheckIfExists(companyId, trackChanges);

		var companyForPatch = _mapper.Map<CompanyForUpdationDto>(company);
		return (companyForPatch, company);
	}

	public async Task SaveChangesForPatchAsync(CompanyForUpdationDto updationDto, Company companyEntity)
	{
		_mapper.Map(updationDto, companyEntity);
		await _repository.SaveAsync();
	}

	public async Task UpdateCompanyAsync(int companyId, CompanyForUpdationDto companyForUpdate, bool trackChanges)
	{
		Company? company = await getCompanyAndCheckIfExists(companyId, trackChanges);

		_mapper.Map(company, companyForUpdate);
		await _repository.SaveAsync();
	}

	public async Task<(IEnumerable<CompanyDto> companies, string ids)> 
		CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
	{
		if (companyCollection is null)
			throw new CompanyCollectionBadRequest();

		var companies = _mapper.Map<IEnumerable<Company>>(companyCollection);
		foreach (var company in companies)
		{
			_repository.Company.CreateCompany(company);
		}
		await _repository.SaveAsync();

		var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
		var ids = string.Join(',', companies.Select(x => x.Id));
		
		return (companiesDto, ids);

	}

	
}
