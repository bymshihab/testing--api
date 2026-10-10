using AutoMapper;
using HoteListing.Api.Data;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Services;

public class CountriesService(HotelListingDbContext context, IMapper mapper) : ICountriesService
{
    public async Task<IEnumerable<GetCountriesDto>> GetCountriesAsync()
    {
        var countries = await context.Countries.AsNoTracking().ToListAsync();
        return mapper.Map<List<GetCountriesDto>>(countries);
    }

    public async Task<GetCountryDto?> GetCountryAsync(int id)
    {
        var country = await context.Countries
            .AsNoTracking()
            .Include(country => country.Hotels)
            .FirstOrDefaultAsync(country => country.CountryId == id);
        return country is null ? null : mapper.Map<GetCountryDto>(country);
    }

    public async Task<GetCountryDto> CreateCountryAsync(CreateCountryDto countryDto)
    {
        var country = mapper.Map<Country>(countryDto);

        context.Countries.Add(country);
        await context.SaveChangesAsync();

        return mapper.Map<GetCountryDto>(country);
    }

    public async Task<bool> UpdateCountryAsync(int id, UpdateCountryDto countryDto)
    {
        var country = await context.Countries.FindAsync(id);
        if (country is null)
        {
            return false;
        }

        mapper.Map(countryDto, country);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCountryAsync(int id)
    {
        var country = await context.Countries.FindAsync(id);
        if (country is null)
        {
            return false;
        }

        context.Countries.Remove(country);
        await context.SaveChangesAsync();
        return true;
    }
}
