using Contracts;
using Entities;
using Service.Contracts;
using Shared.DTOs;

namespace Services;
public class CompanyService: ICompanyService
{
	private readonly IRepositoryManager repository;
	private readonly ILoggerManager logger;

	public CompanyService(IRepositoryManager repository, ILoggerManager logger)
	{
		this.repository = repository;
		this.logger = logger;
	}

	public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
	{
		var companies = repository.Company
			.GetAllCompanies(trackChanges);

		var companiesDto = companies
			.Select(c => new CompanyDto(c.Id, c.Name, $"{c.Address} {c.Country}"));

		return companiesDto;
	}
}
