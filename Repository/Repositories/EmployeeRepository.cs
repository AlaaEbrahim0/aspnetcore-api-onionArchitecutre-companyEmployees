using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System.Linq.Dynamic.Core;
using Repository.Extensions.Utility;
using Shared.RequestFeatures;

namespace Repository.Repositories;
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

	public async Task<PagedList<Employee>> GetEmployeesAsync(int companyId, EmployeeParameters empParams, bool trackChanges)
	{
		if (!empParams.ValidAgeRange)
			throw new EmployeeInvalidAgeRangeException(empParams.MinAge, empParams.MaxAge);


        var employeesQuery = FindByCondition(e => e.CompanyId == companyId, trackChanges)
			.SearchByName(empParams.SearchTerm)
			.Sort(empParams.OrderBy)
			.Filter(empParams.MinAge, empParams.MaxAge);

        var employeesCount = await employeesQuery.CountAsync();

        var employees = await employeesQuery
			.Skip((empParams.PageNumber - 1) * empParams.PageSize)
			.Take(empParams.PageSize)
			.ToListAsync();

		return new PagedList<Employee>(employees, employeesCount, empParams.PageNumber, empParams.PageSize);
	}


}
