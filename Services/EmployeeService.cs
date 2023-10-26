using AutoMapper;
using Contracts;
using Entities;
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

	public EmployeeDto CreateEmployeeForCompany(int companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
	{
		if (!companyExist(companyId, trackChanges))
			throw new CompanyNotFoundException(companyId);

		var employee = mapper.Map<Employee>(employeeForCreation);
		repository.Employee.CreateEmployeeForCompany(companyId, employee);

		repository.Save();

		var employeeDto = mapper.Map<EmployeeDto>(employee);
		return employeeDto;
	}

	public EmployeeDto GetEmployee(int companyId, int employeeId, bool trackChanges)
	{
		if(!companyExist(companyId, trackChanges))
			throw new CompanyNotFoundException(companyId);


		var employee = repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
		if (employee is null)
			throw new EmployeeNotFoundException(employeeId);

		var employeeDto = mapper.Map<EmployeeDto>(employee);

		return employeeDto;
	}

	private bool companyExist(int companyId, bool trackChanges)
	{
		return repository.Company.GetCompany(companyId, trackChanges) is null ? false : true;
	}
	

	public IEnumerable<EmployeeDto> GetEmployees(int companyId, bool trackChanges)
	{
		if (!companyExist(companyId, trackChanges))
			throw new CompanyNotFoundException(companyId);

		var employees = repository.Employee.GetEmployees(companyId, trackChanges);

		var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employees);

		return employeesDto;
	}

	public void DeleteEmployeeForCompany(int companyId, int employeeId, bool trackChanges)
	{
		if (!companyExist(companyId, trackChanges))
			throw new CompanyNotFoundException(companyId);

		var employee = repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
		if (employee is null)
			throw new EmployeeNotFoundException(employeeId);

		repository.Employee.DeleteEmployee(employee);
		repository.Save();
	}

	public void UpdateEmployeeForCompany(int companyId, int employeeId, EmployeeForUpdationDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
	{
		if (!companyExist(companyId, compTrackChanges))
			throw new CompanyNotFoundException(companyId);

		var employee = repository.Employee.GetEmployee(companyId, employeeId, empTrackChanges);
		if (employee is null)
			throw new EmployeeNotFoundException(employeeId);

		mapper.Map(employeeForUpdate, employee);
		repository.Save();
	}

	public (EmployeeForUpdationDto employeeToPatch, Employee employeeEntity)
		GetEmployeeForPatch(int companyId, int id, bool compTrackChanges, bool empTrackChanges)
	{
		if (!companyExist(companyId, compTrackChanges))
			throw new CompanyNotFoundException(companyId);

		var employee = repository.Employee.GetEmployee(companyId, id, empTrackChanges);
		if (employee is null)
			throw new EmployeeNotFoundException(id);

		var employeeToPatch = mapper.Map<EmployeeForUpdationDto>(employee);
		return (employeeToPatch, employee);

	}

	public void SaveChangesForPatch(EmployeeForUpdationDto employeeToPatch, Employee employeeEntity)
	{
		mapper.Map(employeeToPatch, employeeEntity);
		repository.Save();
	}
}
