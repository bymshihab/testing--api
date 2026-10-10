using HoteListing.Api.DTOs.Hotel;
using HotelListing.Api.Results;

namespace HotelListing.Api.Contracts;

public interface IHotelsService
{
    Task<Result<IEnumerable<GetHotelsDto>>> GetHotelsAsync();
    Task<Result<GetHotelDto>> GetHotelAsync(int id);
    Task<Result<GetHotelDto>> CreateHotelAsync(CreateHotelDto hotelDto);
    Task<Result<bool>> UpdateHotelAsync(int id, UpdateHotelDto hotelDto);
    Task<Result<bool>> DeleteHotelAsync(int id);
}
