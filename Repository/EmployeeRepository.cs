using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Exceptions;
using Microsoft.EntityFrameworkCore;

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

	public async Task<IEnumerable<Employee>> GetEmployeesAsync(int companyId, bool trackChanges)
	{
		var employees = await FindByCondition(e => e.CompanyId == companyId, trackChanges)
			.OrderBy(e => e.Name)
			.ToListAsync();

		return employees;

	}

}
