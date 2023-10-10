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

	public Company GetCompany(int id, bool trackChanges)
	{
		var company = FindByCondition(c => c.Id == id, trackChanges)
			.SingleOrDefault();

		return company;
	}
}
