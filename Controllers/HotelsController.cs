using HoteListing.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private static readonly List<Hotel> Hotels =
        [
            new Hotel
            {
                Id = 1,
                Name = "Ocean View Hotel",
                Address = "Cox's Bazar, Bangladesh",
                Rating = "4.5"
            },
            new Hotel
            {
                Id = 2,
                Name = "The Grand Palace",
                Address = "Dhaka, Bangladesh",
                Rating = "4.8"
            },
            new Hotel
            {
                Id = 3,
                Name = "Mountain Retreat",
                Address = "Bandarban, Bangladesh",
                Rating = "4.3"
            }
        ];

        [HttpGet]
        public ActionResult<IEnumerable<Hotel>> GetHotels()
        {
            return Ok(Hotels);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Hotel> GetHotel(int id)
        {
            var hotel = Hotels.FirstOrDefault(hotel => hotel.Id == id);

            if (hotel is null)
            {
                return NotFound(new { message = $"Hotel with ID {id} was not found." });
            }

            return Ok(hotel);
        }

        [HttpPost]
        public IActionResult CreateHotel(Hotel newHotel)
        {
            if (Hotels.Any(hotel => hotel.Id == newHotel.Id))
            {
                return Conflict(new { message = $"Hotel with ID {newHotel.Id} already exists." });
            }

            Hotels.Add(newHotel);

            return CreatedAtAction(nameof(GetHotel), new { id = newHotel.Id }, newHotel);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateHotel(int id, Hotel updatedHotel)
        {
            if (id != updatedHotel.Id)
            {
                return BadRequest(new
                {
                    message = "The route ID must match the hotel ID in the request body."
                });
            }

            var hotelIndex = Hotels.FindIndex(hotel => hotel.Id == id);

            if (hotelIndex == -1)
            {
                return NotFound(new { message = $"Hotel with ID {id} was not found." });
            }

            Hotels[hotelIndex] = updatedHotel;

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteHotel(int id)
        {
            var hotel = Hotels.FirstOrDefault(hotel => hotel.Id == id);

            if (hotel is null)
            {
                return NotFound(new { message = $"Hotel with ID {id} was not found." });
            }

            Hotels.Remove(hotel);

            return NoContent();
        }
    }
}
  
