using AutoMapper;
using CalibrationManagement.Application.DTOs;
using CalibrationManagement.Core.Entities;

namespace CalibrationManagement.Application.Mappings
{
    public class CalibrationMappingProfile : Profile
    {
        public CalibrationMappingProfile()
        {
            CreateMap<CalInfo, CalInfoDto>();
            CreateMap<CreateCalInfoDto, CalInfo>()
                .ForMember(dest => dest.CalId, opt => opt.Ignore())
                .ForMember(dest => dest.CalNo, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore())
                .ForMember(dest => dest.Company, opt => opt.Ignore())
                .ForMember(dest => dest.CalData, opt => opt.Ignore())
                .ForMember(dest => dest.CalStandards, opt => opt.Ignore());
            
            CreateMap<UpdateCalInfoDto, CalInfo>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore())
                .ForMember(dest => dest.Company, opt => opt.Ignore())
                .ForMember(dest => dest.CalData, opt => opt.Ignore())
                .ForMember(dest => dest.CalStandards, opt => opt.Ignore());

            CreateMap<CalData, CalDataDto>();
            CreateMap<CreateCalDataDto, CalData>()
                .ForMember(dest => dest.CalDataId, opt => opt.Ignore())
                .ForMember(dest => dest.Deviation, opt => opt.Ignore())
                .ForMember(dest => dest.PercentDeviation, opt => opt.Ignore())
                .ForMember(dest => dest.PassFail, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore())
                .ForMember(dest => dest.CalInfo, opt => opt.Ignore());
            
            CreateMap<UpdateCalDataDto, CalData>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore())
                .ForMember(dest => dest.CalInfo, opt => opt.Ignore());

            CreateMap<CalStandards, CalStandardsDto>();
            CreateMap<CreateCalStandardsDto, CalStandards>()
                .ForMember(dest => dest.CalStandardId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore())
                .ForMember(dest => dest.CalInfo, opt => opt.Ignore());
            
            CreateMap<UpdateCalStandardsDto, CalStandards>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore())
                .ForMember(dest => dest.CalInfo, opt => opt.Ignore());
        }
    }
}
