using HotelListing.Api.Results;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<T> ToActionResult<T>(Result<T> result)
    {
        return result.IsSuccess ? Ok(result.Value) : ErrorResponse(result.Errors);
    }

    protected ActionResult<T> FromCreatedResult<T>(Result<T> result, string actionName, Func<T, object> routeValues)
    {
        return result.IsSuccess
            ? CreatedAtAction(actionName, routeValues(result.Value!), result.Value)
            : ErrorResponse(result.Errors);
    }

    protected IActionResult FromNoContentResult<T>(Result<T> result)
    {
        return result.IsSuccess ? NoContent() : ErrorResponse(result.Errors);
    }

    private ObjectResult ErrorResponse(Error[] errors)
    {
        return errors.Any(error => error.Code == "NotFound")
            ? NotFound(errors)
            : BadRequest(errors);
    }
}
