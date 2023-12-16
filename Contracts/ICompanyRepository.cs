using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;
public interface ICompanyRepository
{
	Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);
	Task<IEnumerable<Company>> GetAllCompaniesAsync(CompanyParameters companyParameterspc ,bool trackChanges); 
	Task<Company?> GetCompanyAsync (int id, bool trackChanges);
	void CreateCompany (Company company);
	void DeleteCompany(Company company);
}
