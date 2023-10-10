namespace Entities.Exceptions;
public class CompanyNotFoundException : NotFoundException
{
	public CompanyNotFoundException(int companyId)
		: base($"The company with id: {companyId} doesn't exist") 
	{ }
}
