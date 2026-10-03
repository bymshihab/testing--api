using HoteListing.Api.Data;
using HoteListing.Api.DTOs.Hotel;
using HotelListing.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController(HotelListingDbContext context) : ControllerBase
{
    // GET: api/Hotels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetHotelsDto>>> GetHotels()
    {
        var hotesls = await context
            .Hotels.Select(h => new GetHotelsDto(h.Id, h.Name, h.Address, h.Rating, h.CountryId))
            .ToListAsync();
        return Ok(hotesls);
    }

    //GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetHotelDto>> GetHotel(int id)
    {
        var hotel = await context
            .Hotels.Select(h => new GetHotelDto(h.Id, h.Name, h.Address, h.Rating, h.Country!.Name, h.Country!.ShortName))
            .FirstOrDefaultAsync(h => h.Id == id);
        if (hotel == null)
        {
            return NotFound();
        }
        return hotel;
    }

    // PUT: api/Hotels/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto hotelDto)
    {
        if (id != hotelDto.Id)
        {
            return BadRequest();
        }
        var hotel = await context.Hotels.FindAsync(id);
        if (hotel == null)
        {
            return NotFound();
        }
        hotel.Name = hotelDto.Name;
        hotel.Address = hotelDto.Address;
        hotel.Rating = hotelDto.Rating;
        hotel.CountryId = hotelDto.CountryId;

        context.Entry(hotel).State = EntityState.Modified;
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!HotelExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Hotels
    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDTo)
    {
        var newHotel = new Hotel
        {
            Name = hotelDTo.Name,
            Address = hotelDTo.Address,
            Rating = hotelDTo.Rating,
            CountryId = hotelDTo.CountryId
        };
        context.Hotels.Add(newHotel);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetHotel), new { id = newHotel.Id }, newHotel);
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var hotel = await context.Hotels.FindAsync(id);
        if (hotel == null)
        {
            return NotFound();
        }
        context.Hotels.Remove(hotel);
        await context.SaveChangesAsync();
        return NoContent();
    }

    private bool HotelExists(int id)
    {
        return context.Hotels.AnyAsync(hotel => hotel.Id == id).Result;
    }
}
