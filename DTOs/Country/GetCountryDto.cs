using HoteListing.Api.DTOs.Hotel;

public record GetCountryDto(
    int Id,
    string Name, 
    string ShortName,
    List<GetHotelSlimDto> Hotels
);