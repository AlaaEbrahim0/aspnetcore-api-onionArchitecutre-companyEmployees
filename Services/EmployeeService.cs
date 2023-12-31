﻿using System.Dynamic;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Services;

public class EmployeeService : IEmployeeService
{
	private readonly IRepositoryManager repository;
	private readonly ILoggerManager logger;
	private readonly IMapper mapper;
	private readonly IDataShaper<EmployeeDto> dataShaper;

	public EmployeeService(
		IRepositoryManager repository, 
		ILoggerManager logger, 
		IMapper mapper, 
		IDataShaper<EmployeeDto> dataShaper)
	{
		this.repository = repository;
		this.logger = logger;
		this.mapper = mapper;
		this.dataShaper = dataShaper;
	}

	public async Task<EmployeeDto> CreateEmployeeForCompanyAsync
		(int companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
	{
		await companyExistAsync(companyId, trackChanges);

		var employee = mapper.Map<Employee>(employeeForCreation);
		repository.Employee.CreateEmployeeForCompany(companyId, employee);

		await repository.SaveAsync();	
		
		var employeeDto = mapper.Map<EmployeeDto>(employee);
		return employeeDto;
	}

	public async Task<EmployeeDto> GetEmployeeAsync(int companyId, int employeeId, bool trackChanges)
	{
		await companyExistAsync(companyId, trackChanges);

		var employee = await repository.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);

		if (employee is null)
			throw new EmployeeNotFoundException(employeeId);

		var employeeDto = mapper.Map<EmployeeDto>(employee);

		return employeeDto;
	}

	private async Task companyExistAsync(int companyId, bool trackChanges)
	{
		if (await repository.Company.GetCompanyAsync(companyId, trackChanges) is null)
		{
			throw new CompanyNotFoundException(companyId);
		}
	}
	
	public async Task<(IEnumerable<ExpandoObject> employees, MetaData metaData)> GetEmployeesAsync
		(int companyId, EmployeeParameters employeeParameters,bool trackChanges)
	{
		await companyExistAsync(companyId, trackChanges);

		var employeesWithPagingData = await repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);

		var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesWithPagingData);

		return (dataShaper.ShapeData(employeesDto, employeeParameters.Fields), employeesWithPagingData.MetaData);
	}

	public async Task DeleteEmployeeForCompanyAsync(int companyId, int employeeId, bool trackChanges)
	{
		await companyExistAsync(companyId, trackChanges);
			
		var employee = await repository.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);
		if (employee is null)
			throw new EmployeeNotFoundException(employeeId);	

		repository.Employee.DeleteEmployee(employee);
		await repository.SaveAsync();
	}

	public async Task UpdateEmployeeForCompanyAsync
		(int companyId, int employeeId, EmployeeForUpdationDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
    {
        await companyExistAsync(companyId, compTrackChanges);
        var employee = await employeeExistAsync(companyId, employeeId, empTrackChanges);

        mapper.Map(employeeForUpdate, employee);
        await repository.SaveAsync();
    }

    private async Task<Employee?> employeeExistAsync(int companyId, int employeeId, bool empTrackChanges)
    {
        var employee = await repository.Employee.GetEmployeeAsync(companyId, employeeId, empTrackChanges);
        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);
        return employee;
    }

    public async Task<(EmployeeForUpdationDto employeeToPatch, Employee employeeEntity)>
		GetEmployeeForPatchAsync(int companyId, int id, bool compTrackChanges, bool empTrackChanges)
	{
		await companyExistAsync(companyId, compTrackChanges);

		var employee = await repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);
		if (employee is null)
			throw new EmployeeNotFoundException(id);

		var employeeToPatch = mapper.Map<EmployeeForUpdationDto>(employee);
		return (employeeToPatch, employee);

	}

	public async Task SaveChangesForPatchAsync(EmployeeForUpdationDto employeeToPatch, Employee employeeEntity)
	{
		mapper.Map(employeeToPatch, employeeEntity);
		await repository.SaveAsync();
	}
}
