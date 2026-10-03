namespace HoteListing.Api.DTOs.Hotel;

public record GetHotelDto(
    int Id,
    string Name,
    string Address,
    string Rating,
    string Country,
    string? shortName);
