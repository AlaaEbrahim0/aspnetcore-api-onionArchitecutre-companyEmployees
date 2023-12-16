using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Service.Contracts;
using Shared.DTOs;
using Shared.Options;

namespace Services;

public class ServiceManager : IServiceManager
{
	private readonly Lazy<ICompanyService> companyService;
	private readonly Lazy<IEmployeeService> employeeService;
	private readonly Lazy<IAuthenticationService> authenticationService;

	public ServiceManager(IRepositoryManager repository, ILoggerManager logger,
		IMapper mapper, IDataShaper<EmployeeDto> dataShaper, UserManager<AppUser> userManager, IOptions<JwtOptions> options)
	{
		companyService = new(() => new CompanyService(repository, logger, mapper));
		employeeService = new(() => new EmployeeService(repository, logger, mapper, dataShaper));
		authenticationService = new(() => new AuthenticationService(userManager, mapper, options));
	}

	public ICompanyService CompanyService => companyService.Value;
	public IEmployeeService EmployeeService => employeeService.Value;
	public IAuthenticationService AuthenticationService => authenticationService.Value;
}

