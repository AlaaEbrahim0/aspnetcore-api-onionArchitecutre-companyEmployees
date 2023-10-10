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

	public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
	{
		var companies = repository.Company
			.GetAllCompanies(trackChanges);

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
}
