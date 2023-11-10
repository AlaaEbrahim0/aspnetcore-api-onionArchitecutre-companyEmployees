using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository.Repositories;

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

    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges)
    {
        int pageSize = companyParameters.PageSize;
        int pageNum = companyParameters.PageNumber;

        return await 
            FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges)
    {
        return await
            FindByCondition(c => ids.Contains(c.Id), trackChanges)
            .ToListAsync();
    }

    public async Task<Company?> GetCompanyAsync(int id, bool trackChanges)
    {
        var company = await FindByCondition(c => c.Id == id, trackChanges)
            .SingleOrDefaultAsync();

        return company;
    }
}
