using HotelListing.Api.Contracts;
using HotelListing.Api.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController(IUserService userService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisteredUserDto>> Register(RegisterUserDto registerUserDto)
    {
        // // Implement registration logic here
        // var user = new ApplicationUser 
        // {
        //     UserName = registerUserDto.Email,
        //     Email = registerUserDto.Email,
        //     FirstName = registerUserDto.FirstName,
        //     LastName = registerUserDto.LastName
        // };
        // var result = await userManager.CreateAsync(user, registerUserDto.Password);
        // if (!result.Succeeded)
        // {
        //     return BadRequest(result.Errors);
        // }
        // // optional: send confirmation email or perform additional actions  
        // return Ok(new { Message = "User registered successfully" });

        var result = await userService.RegisterAsync(registerUserDto);
        return ToActionResult(result);

    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginUserDto loginUserDto)
    {
        // Implement login logic here
        // var user = await userManager.FindByEmailAsync(loginUserDto.Email);
        // if (user == null)
        // {
        //     return Unauthorized(new { Message = "Invalid email or password" });
        // }
        // var isPasswordValid = await userManager.CheckPasswordAsync(user, loginUserDto.Password);
        // if (!isPasswordValid)
        // {
        //     return Unauthorized(new { Message = "Invalid email or password" });
        // }
        // return Ok(new { Message = "Login successful" });

        var result = await userService.LoginAsync(loginUserDto);
        return ToActionResult(result);
    }
}
