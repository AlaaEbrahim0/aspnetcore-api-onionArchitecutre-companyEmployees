using AutoMapper;
using Entities;
using Shared.DTOs;

namespace CompanyEmployees;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(dest => dest.FullAddress,
            opt => opt.MapFrom(s => string.Join(' ', s.Address, s.Country)));

        CreateMap<CreateCompanyDto, Company>()
            .ForMember(dest => dest.Employees, src => src.MapFrom(opt => opt.Employees));
            
        CreateMap<Employee, EmployeeDto>();

        CreateMap<CreateEmployeeDto, Employee>();

        
    }
}
