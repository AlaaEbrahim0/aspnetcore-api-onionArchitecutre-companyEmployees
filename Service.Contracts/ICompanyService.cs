using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using Entities;
using Shared.DTOs;

namespace Service.Contracts;
public interface ICompanyService
{
	CompanyDto GetCompany(int companyId, bool trackChanges);
	CompanyDto CreateCompany(CompanyForCreationDto companyDto);

	void DeleteCompany(int companyId, bool trackChanges);
	void UpdateCompany(int companyId, CompanyForUpdationDto companyForUpdate, bool trackChanges);

	IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
	IEnumerable<CompanyDto> GetByIds(IEnumerable<int> ids, bool trackChanges);
	(IEnumerable<CompanyDto> companies, string ids)
		CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection);

	(CompanyForUpdationDto companyForPatch, Company companyEntity) GetCompanyForPatch(int companyId, bool trackChanges);
	void SaveChangesForPatch(CompanyForUpdationDto updationDto, Company companyEntity);

}

