using Contracts;
using Entities;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
	public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
	{
	}

	public void DeleteCompany(Company company)
	{
		Delete(company);
	}
	public void CreateCompany(Company company)
	{
		Create(company);
	}

	public IEnumerable<Company> GetAllCompanies(bool trackChanges)
	{
		return this
			.FindAll(trackChanges)
			.OrderBy(c => c.Name)
			.ToList();
	}

	public IEnumerable<Company> GetByIds(IEnumerable<int> ids, bool trackChanges)
	{
		return FindByCondition(c => ids.Contains(c.Id), trackChanges).ToList();
	}

	public Company GetCompany(int id, bool trackChanges)
	{
		var company = FindByCondition(c => c.Id == id, trackChanges)
			.SingleOrDefault();

		return company;
	}
}
