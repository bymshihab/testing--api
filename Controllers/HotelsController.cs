using HoteListing.Api.DTOs.Hotel;
using HotelListing.Api.Contracts;
using HotelListing.Api.Results;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers;

public class HotelsController(IHotelsService hotelsService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetHotelsDto>>> GetHotels()
    {
        var result = await hotelsService.GetHotelsAsync();
        return FromResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetHotelDto>> GetHotel(int id)
    {
        var result = await hotelsService.GetHotelAsync(id);
        return FromResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<GetHotelDto>> PostHotel(CreateHotelDto hotelDto)
    {
        var result = await hotelsService.CreateHotelAsync(hotelDto);
        return FromCreatedResult(result, nameof(GetHotel), hotel => new { id = hotel.Id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto hotelDto)
    {
        if (id != hotelDto.Id)
        {
            return BadRequest(new[] { new Error("BadRequest", "Route ID must match hotel ID.") });
        }

        var result = await hotelsService.UpdateHotelAsync(id, hotelDto);
        return FromNoContentResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var result = await hotelsService.DeleteHotelAsync(id);
        return FromNoContentResult(result);
    }
}
