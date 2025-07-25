using AutoMapper;
using CalibrationManagement.Application.DTOs;
using CalibrationManagement.Core.Entities;

namespace CalibrationManagement.Application.Mappings
{
    public class ToleranceMappingProfile : Profile
    {
        public ToleranceMappingProfile()
        {
            CreateMap<Tolerances, ToleranceDto>();
            CreateMap<CreateToleranceDto, Tolerances>()
                .ForMember(dest => dest.ToleranceId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());
            
            CreateMap<UpdateToleranceDto, Tolerances>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());

            CreateMap<CalTechs, TechnicianDto>();
            CreateMap<CreateTechnicianDto, CalTechs>()
                .ForMember(dest => dest.CalTechId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());
            
            CreateMap<UpdateTechnicianDto, CalTechs>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());
        }
    }
}
