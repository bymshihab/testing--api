using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.DTOs.Auth;
using HotelListing.Api.Results;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Api.Services;

public class UsersService(UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<Result<RegisteredUserDto>> RegisterAsync(RegisterUserDto registeredUserDto)
    {
        var user = new ApplicationUser
        {
            UserName = registeredUserDto.Email,
            Email = registeredUserDto.Email,
            FirstName = registeredUserDto.FirstName,
            LastName = registeredUserDto.LastName
        };

        var result = await userManager.CreateAsync(user, registeredUserDto.Password);
        if (!result.Succeeded)
        {
            return Result<RegisteredUserDto>.Failure(
                result.Errors.Select(error => new Error(error.Code, error.Description)).ToArray());
        }

        var registeredUser = new RegisteredUserDto
        {
            Id = user.Id,
            Email = registeredUserDto.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        return Result<RegisteredUserDto>.Success(registeredUser);
    }

    public async Task<Result<string>> LoginAsync(LoginUserDto loginUserDto)
    {
        var user = await userManager.FindByEmailAsync(loginUserDto.Email);
        if (user == null)
        {
            return Result<string>.Failure(new Error("UserNotFound", "User not found."));
        }

        var passwordValid = await userManager.CheckPasswordAsync(user, loginUserDto.Password);
        if (!passwordValid)
        {
            return Result<string>.Failure(new Error("InvalidPassword", "Invalid password."));
        }

        return Result<string>.Success("Login successful.");
    }   
}
