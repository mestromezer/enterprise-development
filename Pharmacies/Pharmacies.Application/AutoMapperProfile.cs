using AutoMapper;
using Pharmacies.Application.Dto;
using Pharmacies.Model;

namespace Pharmacies.Application;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Pharmacy, PharmacyDto>().ReverseMap();
        ConfigurePositionMapping();
        CreateMap<Price, PriceDto>().ReverseMap();
    }

    private void ConfigurePositionMapping()
    {
        CreateMap<Position, PositionDto>()
            .ForMember(dest => dest.ProductGroupId, opt => opt.MapFrom(src => src.ProductGroup != null ? src.ProductGroup.Id : (int?)null))
            .ForMember(dest => dest.PharmaceuticalGroupIds, opt => opt.MapFrom(src => src.PharmaceuticalGroups.Select(pg => pg.Id).ToList()))
            .ForMember(dest => dest.PharmacyId, opt => opt.MapFrom(src => src.Pharmacy != null ? src.Pharmacy.Number : (int?)null))
            .ForMember(dest => dest.PriceId, opt => opt.MapFrom(src => src.Price != null ? src.Price.Id : (int?)null));

        CreateMap<PositionDto, Position>()
            .ForMember(dest => dest.ProductGroup, opt => opt.Ignore())  // Зависимые сущности загружаются отдельно
            .ForMember(dest => dest.PharmaceuticalGroups, opt => opt.Ignore())  // Нужно загрузить их вручную
            .ForMember(dest => dest.Pharmacy, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.Ignore());
    }
}