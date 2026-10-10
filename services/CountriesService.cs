using AutoMapper;
using HoteListing.Api.Data;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.Results;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Services;

public class CountriesService(HotelListingDbContext context, IMapper mapper) : ICountriesService
{
    public async Task<Result<IEnumerable<GetCountriesDto>>> GetCountriesAsync()
    {
        var countries = await context.Countries.AsNoTracking().ToListAsync();
        return Result<IEnumerable<GetCountriesDto>>.Success(mapper.Map<List<GetCountriesDto>>(countries));
    }

    public async Task<Result<GetCountryDto>> GetCountryAsync(int id)
    {
        var country = await context.Countries
            .AsNoTracking()
            .Include(country => country.Hotels)
            .FirstOrDefaultAsync(country => country.CountryId == id);
        return country is null
            ? Result<GetCountryDto>.NotFound(new Error("NotFound", $"Country {id} was not found."))
            : Result<GetCountryDto>.Success(mapper.Map<GetCountryDto>(country));
    }

    public async Task<Result<GetCountryDto>> CreateCountryAsync(CreateCountryDto countryDto)
    {
        var country = mapper.Map<Country>(countryDto);

        context.Countries.Add(country);
        await context.SaveChangesAsync();

        return Result<GetCountryDto>.Success(mapper.Map<GetCountryDto>(country));
    }

    public async Task<Result<bool>> UpdateCountryAsync(int id, UpdateCountryDto countryDto)
    {
        var country = await context.Countries.FindAsync(id);
        if (country is null)
        {
            return Result<bool>.NotFound(new Error("NotFound", $"Country {id} was not found."));
        }

        mapper.Map(countryDto, country);
        await context.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteCountryAsync(int id)
    {
        var country = await context.Countries.FindAsync(id);
        if (country is null)
        {
            return Result<bool>.NotFound(new Error("NotFound", $"Country {id} was not found."));
        }

        context.Countries.Remove(country);
        await context.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<bool> CountryExistsAsync(int id)
    {
        return await context.Countries.AnyAsync(country => country.CountryId == id);
    }
}
