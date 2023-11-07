using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using Entities;
using Shared.DTOs;

namespace Service.Contracts;
public interface ICompanyService
{
	Task<CompanyDto?> GetCompanyAsync(int companyId, bool trackChanges);
	Task<CompanyDto?> CreateCompanyAsync(CompanyForCreationDto companyDto);

	Task DeleteCompanyAsync(int companyId, bool trackChanges);
	Task UpdateCompanyAsync(int companyId, CompanyForUpdationDto companyForUpdate, bool trackChanges);

	Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);
	Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);
	Task<(IEnumerable<CompanyDto> companies, string ids)>
		CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection);

	Task<(CompanyForUpdationDto companyForPatch, Company companyEntity)> GetCompanyForPatchAsync(int companyId, bool trackChanges);
	Task SaveChangesForPatchAsync(CompanyForUpdationDto updationDto, Company companyEntity);

}

