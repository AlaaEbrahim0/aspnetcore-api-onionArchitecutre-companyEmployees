namespace Entities.Exceptions;
public class CompanyNotFoundException : NotFoundException
{
	public CompanyNotFoundException(int companyId)
		: base($"The company with id: {companyId} doesn't exist") 
	{ }
}


public class EmployeeNotFoundException : NotFoundException
{
	public EmployeeNotFoundException(int employeeId)
		: base($"Employee with id: {employeeId} doesn't exist")
	{ }
}
