using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

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

	public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges)
	{
		return await this
			.FindAll(trackChanges)
			.OrderBy(c => c.Name)
			.ToListAsync();
	}

	public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges)
	{
		return await FindByCondition(c => ids.Contains(c.Id), trackChanges).ToListAsync();
	}

	public async Task<Company?> GetCompanyAsync(int id, bool trackChanges)
	{
		var company = await FindByCondition(c => c.Id == id, trackChanges)
			.SingleOrDefaultAsync();

		return company;
	}
}
