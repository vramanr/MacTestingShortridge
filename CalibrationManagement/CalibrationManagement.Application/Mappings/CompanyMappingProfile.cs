using AutoMapper;
using CalibrationManagement.Application.DTOs;
using CalibrationManagement.Core.Entities;

namespace CalibrationManagement.Application.Mappings
{
    public class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile()
        {
            CreateMap<Company, CompanyDto>();
            CreateMap<CreateCompanyDto, Company>()
                .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore())
                .ForMember(dest => dest.Contacts, opt => opt.Ignore())
                .ForMember(dest => dest.ModelNumbers, opt => opt.Ignore())
                .ForMember(dest => dest.CalInfos, opt => opt.Ignore())
                .ForMember(dest => dest.OrdrStats, opt => opt.Ignore());
            
            CreateMap<UpdateCompanyDto, Company>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore())
                .ForMember(dest => dest.Contacts, opt => opt.Ignore())
                .ForMember(dest => dest.ModelNumbers, opt => opt.Ignore())
                .ForMember(dest => dest.CalInfos, opt => opt.Ignore())
                .ForMember(dest => dest.OrdrStats, opt => opt.Ignore());

            CreateMap<Contact, ContactDto>();
            CreateMap<CreateContactDto, Contact>()
                .ForMember(dest => dest.ContactId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore())
                .ForMember(dest => dest.Company, opt => opt.Ignore());

            CreateMap<ModelNo, ModelNoDto>();
            CreateMap<CreateModelNoDto, ModelNo>()
                .ForMember(dest => dest.ModelNoId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore())
                .ForMember(dest => dest.Company, opt => opt.Ignore());
        }
    }
}
