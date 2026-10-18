using HotelListing.Api.Contracts;
using HotelListing.Api.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers;

[Route("api/[controller]")]
[ApiController] 
[Authorize]
public class CountriesController(ICountriesService countriesService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountriesDto>>> GetCountries()
    {
        var result = await countriesService.GetCountriesAsync();
        return ToActionResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
    {
        var result = await countriesService.GetCountryAsync(id);
        return ToActionResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<GetCountryDto>> PostCountry(CreateCountryDto countryDto)
    {
        var result = await countriesService.CreateCountryAsync(countryDto);
        return FromCreatedResult(result, nameof(GetCountry), country => new { id = country.Id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto countryDto)
    {
        if (id != countryDto.CountryId)
        {
            return BadRequest(new[] { new Error("BadRequest", "Route ID must match country ID.") });
        }

        var result = await countriesService.UpdateCountryAsync(id, countryDto);
        return FromNoContentResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var result = await countriesService.DeleteCountryAsync(id);
        return FromNoContentResult(result);
    }
}
