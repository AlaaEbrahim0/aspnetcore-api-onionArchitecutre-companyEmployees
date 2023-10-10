
using AutoMapper;
using Entities;
using Shared.DTOs;

namespace CompanyEmployees;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForCtorParam("FullAddress",
            opt => opt.MapFrom(s => string.Join(' ', s.Address, s.Country)));


        CreateMap<Employee, EmployeeDto>();
    }
}
