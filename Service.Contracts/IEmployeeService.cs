using Shared.DTOs;

namespace Service.Contracts;

public interface IEmployeeService
{
	IEnumerable<EmployeeDto> GetEmployees(int companyId, bool trackChanges);
	EmployeeDto GetEmployee(int companyId, int employeeId, bool trackChanges);
}
