using AutoMapper;
using Pharmacies.Application.Dto;
using Pharmacies.Model;

namespace Pharmacies.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Position, PositionDto>().ReverseMap();
            CreateMap<Price, PriceDto>().ReverseMap();
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
