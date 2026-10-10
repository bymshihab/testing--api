using HotelListing.Api.Contracts;
using HotelListing.Api.Results;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController(ICountriesService countriesService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountriesDto>>> GetCountries()
    {
        var result = await countriesService.GetCountriesAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToErrorResponse(this);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
    {
        var result = await countriesService.GetCountryAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToErrorResponse(this);
    }

    [HttpPost]
    public async Task<ActionResult<GetCountryDto>> PostCountry(CreateCountryDto countryDto)
    {
        var result = await countriesService.CreateCountryAsync(countryDto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetCountry), new { id = result.Value!.Id }, result.Value)
            : result.ToErrorResponse(this);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto countryDto)
    {
        if (id != countryDto.CountryId)
        {
            return BadRequest(new[] { new Error("BadRequest", "Route ID must match country ID.") });
        }

        var result = await countriesService.UpdateCountryAsync(id, countryDto);
        return result.IsSuccess ? NoContent() : result.ToErrorResponse(this);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var result = await countriesService.DeleteCountryAsync(id);
        return result.IsSuccess ? NoContent() : result.ToErrorResponse(this);
    }
}
