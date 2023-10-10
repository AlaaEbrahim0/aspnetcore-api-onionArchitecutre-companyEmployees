using AutoMapper;
using Contracts;
using Service.Contracts;

namespace Services;

public class EmployeeService : IEmployeeService
{
	private readonly IRepositoryManager repository;
	private readonly ILoggerManager logger;
	private readonly IMapper mapper;


	public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
	{
		this.repository = repository;
		this.logger = logger;
		this.mapper = mapper;
	}
}
