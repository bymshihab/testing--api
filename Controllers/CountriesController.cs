using HoteListing.Api.Data;
using HoteListing.Api.DTOs.Hotel;
using HotelListing.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly HotelListingDbContext _context;

        public CountriesController(HotelListingDbContext context)
        {
            _context = context;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        // GET: api/Countries/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
        {
            var country = await _context
                .Countries.Where(c => c.CountryId == id)
                .Select(c => new GetCountryDto(
                    c.CountryId,
                    c.Name,
                    c.ShortName,
                    c.Hotels.Select(h => new HoteListing.Api.DTOs.Hotel.GetHotelSlimDto(
                            h.Id,
                            h.Name,
                            h.Address,
                            h.Rating
                        ))
                        .ToList()
                ))
                .FirstOrDefaultAsync();

            if (country is null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        // POST: api/Countries
        // POST: api/Countries
        [HttpPost]
        public async Task<ActionResult<GetCountryDto>> PostCountry(CreateCountryDto countryDto)
        {
            var country = new Country { Name = countryDto.Name, ShortName = countryDto.ShortName };

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            var resultDto = new GetCountryDto(
                Id: country.CountryId,
                Name: country.Name,
                ShortName: country.ShortName, 
                Hotels: []
            );

            return CreatedAtAction(nameof(GetCountry), new { id = country.CountryId }, resultDto);
        }

        // PUT: api/Countries/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutCountry(int id, Country country)
        {
            if (id != country.CountryId)
            {
                return BadRequest();
            }

            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country is null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private Task<bool> CountryExists(int id)
        {
            return _context.Countries.AnyAsync(country => country.CountryId == id);
        }
    }
}
