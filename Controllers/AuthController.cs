using HotelListing.Api.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseApiController
{
    public async Task<IActionResult>Register(RegisterUserDto registerUserDto)
    {
        // Implement registration logic here
        return Ok();
    }
}
