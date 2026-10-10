using HotelListing.Api.Results;

namespace HotelListing.Api.Contracts;

public interface ICountriesService
{
    Task<Result<IEnumerable<GetCountriesDto>>> GetCountriesAsync();
    Task<Result<GetCountryDto>> GetCountryAsync(int id);
    Task<Result<GetCountryDto>> CreateCountryAsync(CreateCountryDto countryDto);
    Task<Result<bool>> UpdateCountryAsync(int id, UpdateCountryDto countryDto);
    Task<Result<bool>> DeleteCountryAsync(int id);
}
