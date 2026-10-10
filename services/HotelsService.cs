using AutoMapper;
using HoteListing.Api.Data;
using HoteListing.Api.DTOs.Hotel;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Services;

public class HotelsService(HotelListingDbContext context, IMapper mapper) : IHotelsService
{
    public async Task<IEnumerable<GetHotelsDto>> GetHotelsAsync()
    {
        var hotels = await context.Hotels.AsNoTracking().ToListAsync();
        return mapper.Map<List<GetHotelsDto>>(hotels);
    }

    public async Task<GetHotelDto?> GetHotelAsync(int id)
    {
        var hotel = await context
            .Hotels.Where(hotel => hotel.Id == id)
            .Include(hotel => hotel.Country)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return hotel is null ? null : mapper.Map<GetHotelDto>(hotel);
    }

    public async Task<GetHotelDto> CreateHotelAsync(CreateHotelDto hotelDto)
    {
        var hotel = mapper.Map<Hotel>(hotelDto);

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

        mapper.Map(hotelDto, hotel);
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
