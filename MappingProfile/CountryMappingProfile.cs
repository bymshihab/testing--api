using AutoMapper;
using HoteListing.Api.Data;

namespace HotelListing.Api.Mapping;

public class CountryMappingProfile : Profile
{
    public CountryMappingProfile()
    {
        CreateMap<Country, GetCountriesDto>();
        CreateMap<Country, GetCountryDto>()
            .ForCtorParam(nameof(GetCountryDto.Id), options => options.MapFrom(country => country.CountryId));

        CreateMap<CreateCountryDto, Country>()
            .ForMember(country => country.CountryId, options => options.Ignore())
            .ForMember(country => country.Hotels, options => options.Ignore());

        CreateMap<UpdateCountryDto, Country>()
            .ForMember(country => country.CountryId, options => options.Ignore())
            .ForMember(country => country.Hotels, options => options.Ignore());
    }
}
 