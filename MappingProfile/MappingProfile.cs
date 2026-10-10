using AutoMapper;
using HoteListing.Api.Data;
using HoteListing.Api.DTOs.Hotel;

namespace HotelListing.Api.Mapping;

public class HotelMappingProfile : Profile
{
    public HotelMappingProfile()
    {
        CreateMap<Hotel, GetHotelsDto>();
        CreateMap<Hotel, GetHotelSlimDto>();
        CreateMap<Hotel, GetHotelDto>()
            .ForMember(dto => dto.Country, options => options.MapFrom<CountryNameResolver>())
            .ForMember(dto => dto.shortName, options => options.MapFrom(hotel => hotel.Country == null ? null : hotel.Country.ShortName));

        CreateMap<CreateHotelDto, Hotel>()
            .ForMember(hotel => hotel.Id, options => options.Ignore())
            .ForMember(hotel => hotel.Country, options => options.Ignore());
        CreateMap<UpdateHotelDto, Hotel>()
            .ForMember(hotel => hotel.Id, options => options.Ignore())
            .ForMember(hotel => hotel.Country, options => options.Ignore());
    }
}

public class CountryNameResolver : IValueResolver<Hotel, GetHotelDto, string>
{
    public string Resolve(Hotel source, GetHotelDto destination, string destMember, ResolutionContext context)
    {
        return source.Country?.Name ?? string.Empty;
    }
}
