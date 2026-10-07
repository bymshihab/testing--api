using HoteListing.Api.Data;
using HoteListing.Api.DTOs.Hotel;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Services;

public class HotelsService(HotelListingDbContext context) : IHotelsService
{
    public async Task<IEnumerable<GetHotelsDto>> GetHotelsAsync()
    {
        return await context.Hotels
            .AsNoTracking()
            .Select(hotel => new GetHotelsDto(
                hotel.Id, hotel.Name!, hotel.Address, hotel.Rating, hotel.CountryId))
            .ToListAsync();
    }

    public async Task<GetHotelDto?> GetHotelAsync(int id)
    {
        return await context.Hotels
            .AsNoTracking()
            .Where(hotel => hotel.Id == id)
            .Select(hotel => new GetHotelDto(
                hotel.Id, hotel.Name!, hotel.Address, hotel.Rating,
                hotel.Country!.Name!, hotel.Country.ShortName))
            .FirstOrDefaultAsync();
    }

    public async Task<GetHotelDto> CreateHotelAsync(CreateHotelDto hotelDto)
    {
        var hotel = new Hotel
        {
            Name = hotelDto.Name,
            Address = hotelDto.Address,
            Rating = hotelDto.Rating,
            CountryId = hotelDto.CountryId
        };

        context.Hotels.Add(hotel);
        await context.SaveChangesAsync();

        return (await GetHotelAsync(hotel.Id))!;
    }

    public async Task<bool> UpdateHotelAsync(int id, UpdateHotelDto hotelDto)
    {
        var hotel = await context.Hotels.FindAsync(id);
        if (hotel is null)
        {
            return false;
        }

        hotel.Name = hotelDto.Name;
        hotel.Address = hotelDto.Address;
        hotel.Rating = hotelDto.Rating;
        hotel.CountryId = hotelDto.CountryId;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteHotelAsync(int id)
    {
        var hotel = await context.Hotels.FindAsync(id);
        if (hotel is null)
        {
            return false;
        }

        context.Hotels.Remove(hotel);
        await context.SaveChangesAsync();
        return true;
    }
}
