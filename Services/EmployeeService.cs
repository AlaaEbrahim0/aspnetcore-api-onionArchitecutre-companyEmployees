using Contracts;
using Service.Contracts;

namespace Services;

public class EmployeeService : IEmployeeService
{
	private readonly IRepositoryManager repository;
	private readonly ILoggerManager logger;

	public EmployeeService(IRepositoryManager repository, ILoggerManager logger)
	{
		this.repository = repository;
		this.logger = logger;
	}
}
