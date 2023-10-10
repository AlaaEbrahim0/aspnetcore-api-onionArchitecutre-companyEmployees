using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using Shared.DTOs;

namespace Service.Contracts;
public interface ICompanyService
{
	IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
	CompanyDto GetCompany(int companyId, bool trackChanges);
}
