using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Contracts;
public interface IEmployeeRepository
{
	IEnumerable<Employee> GetEmployees(int companyId, bool trackChanges);
	Employee GetEmployee(int companyId, int employeeId, bool trackChanges);
}
