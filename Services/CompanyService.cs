using System.Reflection.Metadata;
using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DTOs;

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

	public CompanyDto CreateCompany(CompanyForCreationDto CompanyForCreationDto)
	{
		var company = mapper.Map<Company>(CompanyForCreationDto);

		repository.Company.CreateCompany(company);
		repository.Save();

		var returnCompanyDto = mapper.Map<CompanyDto>(company);

		return returnCompanyDto;	
	}

	public void DeleteCompany(int companyId, bool trackChanges)
	{
		var company = repository.Company.GetCompany(companyId, trackChanges	);
		if (company is null)
			throw new CompanyNotFoundException(companyId);

		repository.Company.DeleteCompany(company);
		repository.Save();
	}

	public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
	{
		var companies = repository.Company
			.GetAllCompanies(trackChanges);

		var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
		return companiesDto;
	}

	public IEnumerable<CompanyDto> GetByIds(IEnumerable<int> ids, bool trackChanges)
	{
		if (ids is null)
			throw new IdParametersBadRequestException();

		var companies = repository.Company.GetByIds(ids, trackChanges);
		if (ids.Count() != companies.Count())
			throw new CollectionByIdsBadRequestException();

		var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
		return companiesDto;
	}

	public CompanyDto GetCompany(int companyId, bool trackChanges)
	{
		var company = repository.Company.GetCompany(companyId, trackChanges);

		if (company is null)
			throw new CompanyNotFoundException(companyId);

		var companyDto = mapper.Map<CompanyDto>(company);

		return companyDto;
	}

	public (CompanyForUpdationDto, Company) GetCompanyForPatch(int companyId, bool trackChanges)
	{
		var company = repository.Company.GetCompany(companyId, trackChanges);

		if (company is null)
			throw new CompanyNotFoundException(companyId);

		var companyForPatch = mapper.Map<CompanyForUpdationDto>(company);
		return (companyForPatch, company);
	}

	public void SaveChangesForPatch(CompanyForUpdationDto updationDto, Company companyEntity)
	{
		mapper.Map(updationDto, companyEntity);
		repository.Save();
	}

	public void UpdateCompany(int companyId, CompanyForUpdationDto companyForUpdate, bool trackChanges)
	{
		var company = repository.Company.GetCompany(companyId, trackChanges);
		if (company is null)
			throw new CompanyNotFoundException(companyId);

		mapper.Map(company, companyForUpdate);
		repository.Save();
	}

	(IEnumerable<CompanyDto> companies, string ids) ICompanyService.CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
	{
		if (companyCollection is null)
			throw new CompanyCollectionBadRequest();

		var companies = mapper.Map<IEnumerable<Company>>(companyCollection);
		foreach (var company in companies)
		{
			repository.Company.CreateCompany(company);
		}
		repository.Save();

		var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
		var ids = string.Join(',', companies.Select(x => x.Id));
		
		return (companiesDto, ids);

	}
}
