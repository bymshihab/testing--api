using HoteListing.Api.DTOs.Hotel;
using HotelListing.Api.Contracts;
using HotelListing.Api.Results;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController(IHotelsService hotelsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetHotelsDto>>> GetHotels()
    {
        var result = await hotelsService.GetHotelsAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToErrorResponse(this);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetHotelDto>> GetHotel(int id)
    {
        var result = await hotelsService.GetHotelAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToErrorResponse(this);
    }

    [HttpPost]
    public async Task<ActionResult<GetHotelDto>> PostHotel(CreateHotelDto hotelDto)
    {
        var result = await hotelsService.CreateHotelAsync(hotelDto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetHotel), new { id = result.Value!.Id }, result.Value)
            : result.ToErrorResponse(this);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto hotelDto)
    {
        if (id != hotelDto.Id)
        {
            return BadRequest(new[] { new Error("BadRequest", "Route ID must match hotel ID.") });
        }

        var result = await hotelsService.UpdateHotelAsync(id, hotelDto);
        return result.IsSuccess ? NoContent() : result.ToErrorResponse(this);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var result = await hotelsService.DeleteHotelAsync(id);
        return result.IsSuccess ? NoContent() : result.ToErrorResponse(this);
    }
}
