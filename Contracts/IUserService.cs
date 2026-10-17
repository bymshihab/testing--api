using HotelListing.Api.DTOs.Auth;
using HotelListing.Api.Results;

namespace HotelListing.Api.Contracts;

public interface IUserService
{
    Task<Result<RegisteredUserDto>> RegisterAsync(RegisterUserDto registeredUserDto);
    Task<Result<string>> LoginAsync(LoginUserDto loginUserDto);
}