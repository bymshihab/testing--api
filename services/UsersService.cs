using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.DTOs.Auth;
using HotelListing.Api.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using JwtClaims = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace HotelListing.Api.Services;

public class UsersService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    : IUserService
{
    public async Task<Result<RegisteredUserDto>> RegisterAsync(RegisterUserDto registeredUserDto)
    {
        var user = new ApplicationUser
        {
            UserName = registeredUserDto.Email,
            Email = registeredUserDto.Email,
            FirstName = registeredUserDto.FirstName,
            LastName = registeredUserDto.LastName,
        };

        var result = await userManager.CreateAsync(user, registeredUserDto.Password);
        if (!result.Succeeded)
        {
            return Result<RegisteredUserDto>.Failure(
                result.Errors.Select(error => new Error(error.Code, error.Description)).ToArray()
            );
        }

        var registeredUser = new RegisteredUserDto
        {
            Id = user.Id,
            Email = registeredUserDto.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
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

        // Issue a JWT token or perform any other login-related operations here.
        var token = await GenerateToken(user);

        return Result<string>.Success(token);
    }

    private async Task<string> GenerateToken(ApplicationUser user)
    {
        // Implement JWT token generation logic here.
        // You can use libraries like System.IdentityModel.Tokens.Jwt to create the token.
        // Return the generated token as a string.

        //set Basic claims for the token
        var claims = new List<Claim>
        {
            new(JwtClaims.Sub, user.Id),
            new(JwtClaims.Email, user.Email!),
            new(JwtClaims.Name, $"{user.FirstName} {user.LastName}"),
        };

        // set user roles as claims
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
        claims = claims.Union(roleClaims).ToList();

        // set JWT Key Credentials
        var security = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["jwtSettings:Key"]!)
        );
        var credentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);

        // create encoded JWT token
        var token = new JwtSecurityToken(
            issuer: configuration["jwtSettings:Issuer"],
            audience: configuration["jwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                configuration.GetValue<int>("jwtSettings:DurationInMinutes")
            ), // Set token expiration time
            signingCredentials: credentials
        );
        // Return the token value.
        return new JwtSecurityTokenHandler().WriteToken(token); // Return the generated token.
    }
}
