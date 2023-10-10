using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace Repository;
public class RepositoryManager : IRepositoryManager
{
	private readonly RepositoryContext repositoryContext;
	private readonly Lazy<ICompanyRepository> companyRepository;
	private readonly Lazy<IEmployeeRepository> employeeRepository;

	public RepositoryManager(RepositoryContext repositoryContext)
	{
		this.repositoryContext = repositoryContext;
		companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(repositoryContext));
		employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(repositoryContext));
	}

	public IEmployeeRepository Employee => employeeRepository.Value;
	public ICompanyRepository Company => companyRepository.Value;

	public void Save() => repositoryContext.SaveChanges();
}
