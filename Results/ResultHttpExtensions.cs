using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Results;

public static class ResultHttpExtensions
{
    public static ObjectResult ToErrorResponse<T>(this Result<T> result, ControllerBase controller)
    {
        return result.Errors.Any(error => error.Code == "NotFound")
            ? controller.NotFound(result.Errors)
            : controller.BadRequest(result.Errors);
    }
}
