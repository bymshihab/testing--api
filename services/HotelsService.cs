using AutoMapper;
using HoteListing.Api.Data;
using HoteListing.Api.DTOs.Hotel;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.Results;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Services;

public class HotelsService(HotelListingDbContext context, IMapper mapper) : IHotelsService
{
    public async Task<Result<IEnumerable<GetHotelsDto>>> GetHotelsAsync()
    {
        var hotels = await context.Hotels.AsNoTracking().ToListAsync();
        return Result<IEnumerable<GetHotelsDto>>.Success(mapper.Map<List<GetHotelsDto>>(hotels));
    }

    public async Task<Result<GetHotelDto>> GetHotelAsync(int id)
    {
        var hotel = await context
            .Hotels.Where(hotel => hotel.Id == id)
            .Include(hotel => hotel.Country)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return hotel is null
            ? Result<GetHotelDto>.NotFound(new Error("NotFound", $"Hotel {id} was not found."))
            : Result<GetHotelDto>.Success(mapper.Map<GetHotelDto>(hotel));
    }

    public async Task<Result<GetHotelDto>> CreateHotelAsync(CreateHotelDto hotelDto)
    {
        if (!await context.Countries.AnyAsync(country => country.CountryId == hotelDto.CountryId))
        {
            return Result<GetHotelDto>.Failure(new Error("BadRequest", $"Country {hotelDto.CountryId} was not found."));
        }

        var hotel = mapper.Map<Hotel>(hotelDto);

        context.Hotels.Add(hotel);
        await context.SaveChangesAsync();
        return await GetHotelAsync(hotel.Id);
    }

    public async Task<Result<bool>> UpdateHotelAsync(int id, UpdateHotelDto hotelDto)
    {
        var hotel = await context.Hotels.FindAsync(id);
        if (hotel is null)
        {
            return Result<bool>.NotFound(new Error("NotFound", $"Hotel {id} was not found."));
        }

        if (!await context.Countries.AnyAsync(country => country.CountryId == hotelDto.CountryId))
        {
            return Result<bool>.Failure(new Error("BadRequest", $"Country {hotelDto.CountryId} was not found."));
        }

        mapper.Map(hotelDto, hotel);
        await context.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteHotelAsync(int id)
    {
        var hotel = await context.Hotels.FindAsync(id);
        if (hotel is null)
        {
            return Result<bool>.NotFound(new Error("NotFound", $"Hotel {id} was not found."));
        }

        context.Hotels.Remove(hotel);
        await context.SaveChangesAsync();
        return Result<bool>.Success(true);
    }
}
