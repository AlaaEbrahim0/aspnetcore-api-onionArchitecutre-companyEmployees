using AutoMapper;
using Entities.Models;
using Shared.DTOs;

namespace CompanyEmployees;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(dest => dest.FullAddress,
            opt => opt.MapFrom(s => string.Join(' ', s.Address, s.Country)));

        CreateMap<CompanyForCreationDto, Company>()
            .ForMember(dest => dest.Employees, src => src.MapFrom(opt => opt.Employees));
            
        CreateMap<Employee, EmployeeDto>();

		CreateMap<EmployeeForCreationDto, Employee>();

        CreateMap<EmployeeForUpdationDto, Employee>();
        CreateMap<EmployeeForUpdationDto, Employee>().ReverseMap();

        CreateMap<CompanyForUpdationDto, Company>();
        CreateMap<CompanyForUpdationDto, Company>().ReverseMap();

        CreateMap<RegisterationDto, AppUser>();


 
	}
}
