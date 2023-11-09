using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository;
public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
	public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
	{
	}

	public void CreateEmployeeForCompany(int companyId, Employee employee)
	{
		employee.CompanyId = companyId;
		Create(employee);
	}

	public void DeleteEmployee(Employee employee)
	{
		Delete(employee);
	}

	public async Task<Employee?> GetEmployeeAsync(int companyId, int employeeId, bool trackChanges)
	{
		var employee = await FindByCondition(e => e.Id == employeeId && e.CompanyId == companyId, trackChanges)
			.SingleOrDefaultAsync();

		return employee;
	}

	public async Task<IEnumerable<Employee>> GetEmployeesAsync(int companyId, EmployeeParameters employeeParameters ,bool trackChanges)
	{
		if (!employeeParameters.ValidAgeRange)
		{
			throw new EmployeeInvalidAgeRangeException
				(employeeParameters.MinAge, employeeParameters.MaxAge);
		}

		var pageSize = employeeParameters.PageSize;
		var pageNum = employeeParameters.PageNumber;
		var minAge = employeeParameters.MinAge;

		var employees = await this
			.FindByCondition(e => e.CompanyId == companyId, trackChanges)
			.SearchByName(employeeParameters.SearchTerm)
			.Filter(employeeParameters.MinAge, employeeParameters.MaxAge)
			.Skip((pageNum - 1) * pageSize)
			.Take(pageSize)
			.OrderBy(e => e.Name)
			.ToListAsync();

		return employees;

	}

}
