namespace HoteListing.Api.DTOs.Hotel;

public record GetHotelSlimDto(
    int Id, 
    string Name, 
    string Address, 
    string Rating
);