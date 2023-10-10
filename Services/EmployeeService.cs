using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DTOs;

namespace Services;

public class EmployeeService : IEmployeeService
{
	private readonly IRepositoryManager repository;
	private readonly ILoggerManager logger;
	private readonly IMapper mapper;


	public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
	{
		this.repository = repository;
		this.logger = logger;
		this.mapper = mapper;
	}

	public EmployeeDto GetEmployee(int companyId, int employeeId, bool trackChanges)
	{
		var company = repository.Company.GetCompany(companyId, trackChanges);
		if (company is null)
			throw new CompanyNotFoundException(companyId);

		var employee = repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
		if (employee is null)
			throw new EmployeeNotFoundException(employeeId);

		var employeeDto = mapper.Map<EmployeeDto>(employee);

		return employeeDto;
	}


	public IEnumerable<EmployeeDto> GetEmployees(int companyId, bool trackChanges)
	{

		var company = repository.Company.GetCompany(companyId, trackChanges);
		if (company is null)
			throw new CompanyNotFoundException(companyId);

		var employees = repository.Employee.GetEmployees(companyId, trackChanges);

		var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employees);

		return employeesDto;
	}
}
