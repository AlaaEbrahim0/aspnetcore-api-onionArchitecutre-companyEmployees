using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures;
public class CompanyParameters : RequestParameters
{
    public string Address { get; set; } = string.Empty;
}
