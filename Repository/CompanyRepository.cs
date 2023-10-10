using Contracts;
using Entities;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
	public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
	{
	}

	public IEnumerable<Company> GetAllCompanies(bool trackChanges)
	{
		return this
			.FindAll(trackChanges)
			.OrderBy(c => c.Name)
			.ToList();
	}

}
