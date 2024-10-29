using AutoMapper;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Dto.Reference;
using Pharmacies.Model;
using Pharmacies.Model.Reference;

namespace Pharmacies.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Pharmacy, PharmacyDto>().ReverseMap();
            CreateMap<Price, PriceDto>().ReverseMap();
            CreateMap<PharmaceuticalGroup, PharmaceuticalGroupDto>().ReverseMap();
            CreateMap<ProductGroup, ProductGroupDto>().ReverseMap();
            CreateMap<PharmaceuticalGroupReference, PharmaceuticalGroupReferenceDto>().ReverseMap();
            ConfigurePositionMapping();
        }

        private void ConfigurePositionMapping()
        {
            CreateMap<Position, PositionDto>();

            CreateMap<PositionDto, Position>()
                .ForMember(dest => dest.ProductGroup, opt => opt.Ignore())
                .ForMember(dest => dest.Pharmacy, opt => opt.Ignore())
                .ForMember(dest => dest.Price, opt => opt.Ignore());
        }
    }
}
