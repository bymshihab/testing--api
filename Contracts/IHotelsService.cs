using HoteListing.Api.DTOs.Hotel;

namespace HotelListing.Api.Contracts;

public interface IHotelsService
{
    Task<IEnumerable<GetHotelsDto>> GetHotelsAsync();
    Task<GetHotelDto?> GetHotelAsync(int id);
    Task<GetHotelDto> CreateHotelAsync(CreateHotelDto hotelDto);
    Task<bool> UpdateHotelAsync(int id, UpdateHotelDto hotelDto);
    Task<bool> DeleteHotelAsync(int id);
}
