using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts;
public interface IRepositoryManager
{
	IEmployeeRepository Employee { get; }
	ICompanyRepository Company { get; }
	void Save();
}
