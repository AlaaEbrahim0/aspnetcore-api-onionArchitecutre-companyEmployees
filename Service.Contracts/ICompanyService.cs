using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using Shared.DTOs;

namespace Service.Contracts;
public interface ICompanyService
{
	IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
	CompanyDto GetCompany(int companyId, bool trackChanges);
	CompanyDto CreateCompany(CreateCompanyDto companyDto);
	IEnumerable<CompanyDto> GetByIds(IEnumerable<int> ids, bool trackChanges);
	(IEnumerable<CompanyDto> companies, string ids)
		CreateCompanyCollection(IEnumerable<CreateCompanyDto> companyCollection);
	void DeleteCompany(int companyId, bool trackChanges);
}

