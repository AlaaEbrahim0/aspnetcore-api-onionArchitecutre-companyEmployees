using AutoMapper;
using Contracts;
using Service.Contracts;
using Shared.DTOs;

namespace Services;

public class ServiceManager : IServiceManager
{
	private readonly Lazy<ICompanyService> companyService;
	private readonly Lazy<IEmployeeService> employeeService;

	public ServiceManager(IRepositoryManager repository, ILoggerManager logger,
		IMapper mapper, IDataShaper<EmployeeDto> dataShaper)
	{
		companyService = new(() => new CompanyService(repository, logger, mapper));
		employeeService = new(() => new EmployeeService(repository, logger, mapper, dataShaper));
	}

	public ICompanyService CompanyService => companyService.Value;
	public IEmployeeService EmployeeService => employeeService.Value;
}

