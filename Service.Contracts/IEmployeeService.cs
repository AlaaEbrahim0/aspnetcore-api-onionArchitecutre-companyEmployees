using Entities;
using Shared.DTOs;

namespace Service.Contracts;

public interface IEmployeeService
{
	IEnumerable<EmployeeDto> GetEmployees(int companyId, bool trackChanges);
	EmployeeDto GetEmployee(int companyId, int employeeId, bool trackChanges);
	EmployeeDto CreateEmployeeForCompany(int companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);
	void DeleteEmployeeForCompany(int companyId, int employeeId, bool trackChanges);
	void UpdateEmployeeForCompany(int companyId, int employeeId, EmployeeForUpdationDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);

	(EmployeeForUpdationDto employeeToPatch, Employee employeeEntity) 
		GetEmployeeForPatch(int companyId, int id, bool compTrackChanges, bool empTrackChanges); 
	
	void SaveChangesForPatch(EmployeeForUpdationDto employeeToPatch, Employee employeeEntity);
}
