using HoteListing.Api.Data;
using HoteListing.Api.DTOs.Hotel;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Services;

public class CountriesService(HotelListingDbContext context) : ICountriesService
{
    public async Task<IEnumerable<GetCountriesDto>> GetCountriesAsync()
    {
        return await context.Countries
            .AsNoTracking()
            .Select(country => new GetCountriesDto(
                country.Name!, country.ShortName!, country.CountryId))
            .ToListAsync();
    }

    public async Task<GetCountryDto?> GetCountryAsync(int id)
    {
        return await context.Countries
            .AsNoTracking()
            .Where(country => country.CountryId == id)
            .Select(country => new GetCountryDto(
                country.CountryId,
                country.Name!,
                country.ShortName!,
                country.Hotels!.Select(hotel => new GetHotelSlimDto(
                    hotel.Id, hotel.Name!, hotel.Address, hotel.Rating)).ToList()))
            .FirstOrDefaultAsync() ?? null;
    }

    public async Task<GetCountryDto> CreateCountryAsync(CreateCountryDto countryDto)
    {
        var country = new Country
        {
            Name = countryDto.Name,
            ShortName = countryDto.ShortName
        };

        context.Countries.Add(country);
        await context.SaveChangesAsync();

        return new GetCountryDto(country.CountryId, country.Name!, country.ShortName!, []);
    }

    public async Task<bool> UpdateCountryAsync(int id, UpdateCountryDto countryDto)
    {
        var country = await context.Countries.FindAsync(id);
        if (country is null)
        {
            return false;
        }

        country.Name = countryDto.Name;
        country.ShortName = countryDto.ShortName;
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
