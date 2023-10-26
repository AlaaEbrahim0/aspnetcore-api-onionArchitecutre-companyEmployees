using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Exceptions;

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

	public Employee GetEmployee(int companyId, int employeeId, bool trackChanges)
	{
		var employee = FindByCondition(e => e.Id == employeeId && e.CompanyId == companyId, trackChanges)
			.SingleOrDefault();

		return employee;
	}

	public IEnumerable<Employee> GetEmployees(int companyId, bool trackChanges)
	{
		var employees = FindByCondition(e => e.CompanyId == companyId, trackChanges)
			.OrderBy(e => e.Name)
			.ToList();

		return employees;

	}

}
