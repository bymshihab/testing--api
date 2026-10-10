namespace HoteListing.Api.DTOs.Hotel;

public record GetHotelDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Rating { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string? shortName { get; init; }
}
