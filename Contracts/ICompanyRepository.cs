using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Contracts;
public interface ICompanyRepository
{
	IEnumerable<Company> GetAllCompanies(bool trackChanges); 
	Company GetCompany (int id, bool trackChanges);

}
