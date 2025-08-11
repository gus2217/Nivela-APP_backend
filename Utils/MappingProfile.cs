using AutoMapper;
using NivelaService.Models.Domain;
using NivelaService.Models.Dto;

namespace NivelaService.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Vendor mappings
            CreateMap<UpdateVendorDto, Vendor>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Services, opt => opt.Ignore())
                .ForMember(dest => dest.Socials, opt => opt.Ignore()); // ensure Socials are manually updated

            CreateMap<CreateVendorDto, Vendor>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Services, opt => opt.Ignore())
                .ForMember(dest => dest.Socials, opt => opt.Ignore()); // same here for full manual control

            CreateMap<Vendor, VendorToDisplayDto>()
                .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Services))
                .ForMember(dest => dest.Socials, opt => opt.MapFrom(src => src.Socials))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            // Socials
            CreateMap<CreateSocialsDto, Social>();
            CreateMap<UpdateSocialDto, Social>();
            CreateMap<Social, SocialDto>();

            // Services
            CreateMap<Service, ServiceDto>();

            // Images
            CreateMap<VendorImage, VendorImageDto>()
                .ForMember(dest => dest.Base64Content, opt => opt.MapFrom(src => Convert.ToBase64String(src.Content)));
        }
    }
}
