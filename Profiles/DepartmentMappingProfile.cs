using AutoMapper;
using LabApi.DTO;
using LabApi.Models;

namespace LabApi.Profiles
{
    public class DepartmentMappingProfile:Profile
    {
       public DepartmentMappingProfile() 
        {
        CreateMap<Department, DepartmentDTO>().
                ForMember(dest=>dest.DeptName,opt=>opt.MapFrom(src=>src.Name)).
                ForMember(dest=>dest.StudentSNames, opt => opt.MapFrom(src => src.Students.Select(s => s.Name))).
                ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.ManagerName)).
                ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Students.Count)).
                ForMember(dest => dest.Message, opt => opt.MapFrom(src => "Department found"));
        }
    }
}
