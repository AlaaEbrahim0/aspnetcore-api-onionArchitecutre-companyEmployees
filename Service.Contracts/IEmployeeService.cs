using System.Dynamic;
using Entities;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IEmployeeService
{
	Task<IEnumerable<ExpandoObject>> GetEmployeesAsync(int companyId, EmployeeParameters employeeParameters, bool trackChanges);
	Task<EmployeeDto> GetEmployeeAsync(int companyId, int employeeId, bool trackChanges);
	Task<EmployeeDto> CreateEmployeeForCompanyAsync(int companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);
	Task DeleteEmployeeForCompanyAsync(int companyId, int employeeId, bool trackChanges);
	Task UpdateEmployeeForCompanyAsync(int companyId, int employeeId, EmployeeForUpdationDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);

	Task<(EmployeeForUpdationDto employeeToPatch, Employee employeeEntity)>
		GetEmployeeForPatchAsync(int companyId, int id, bool compTrackChanges, bool empTrackChanges); 
	
	Task SaveChangesForPatchAsync(EmployeeForUpdationDto employeeToPatch, Employee employeeEntity);
}
