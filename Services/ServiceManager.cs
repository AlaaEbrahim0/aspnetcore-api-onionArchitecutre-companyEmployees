using Contracts;
using Service.Contracts;

namespace Services;

public class ServiceManager : IServiceManager
{
	private readonly Lazy<ICompanyService> companyService;
	private readonly Lazy<IEmployeeService> employeeService;

	public ServiceManager(IRepositoryManager repository, ILoggerManager logger)
	{
		companyService = new(() => new CompanyService(repository, logger));
		employeeService = new(() => new EmployeeService(repository, logger));
	}

	public ICompanyService CompanyService => companyService.Value;
	public IEmployeeService EmployeeService => employeeService.Value;
}

