using AutoMapper;
using LabApi.DTO;
using LabApi.Models;

namespace LabApi.Profiles
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.deptName, opt => opt.MapFrom(src => src.Department!.Name))
                .ForMember(dest => dest.skill, opt => opt.MapFrom(src => "problem solving"));
        }
    }
}